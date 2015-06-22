using Percurrentis.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;

namespace Percurrentis.AD_classes
{
    public class ADservices
    {

        private static object lockingObject = new object();
        private volatile static ADservices singletonObject;

        private Dictionary<string, UserAC> userDictonary = new Dictionary<string, UserAC>();
        private List<string> departmentList = new List<string>();

        /// <summary>
        /// Singleton use examble: 
        /// ADservices ADservices = ADservices.InstanceCreation();
        /// List<UserAC> List = ADservices.GetUsers();
        /// </summary>
        /// <returns></returns>
        public static ADservices InstanceCreation()
        {
            if (singletonObject == null)
            {
                lock (lockingObject)
                {
                    if (singletonObject == null)
                    {
                        singletonObject = new ADservices();
                    }
                }
            }
            return singletonObject;
        }

        /// <summary>
        /// Used for LDAP
        /// </summary>
        /// <returns></returns>
        private static DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry oDE;
            oDE = new DirectoryEntry("LDAP://OU=Romania,DC=CSiRO,DC=local");
            return oDE;
        }

        /// <summary>
        /// Since this is used as a singleton the object doesn't always have to be filled.
        /// Make sure this is used if you want to use the userDictonary.
        /// </summary>
        private void FillUserDictonaryIfNeeded()
        {
            if (userDictonary.Count == 0)
            {
                FillUserDictonary();
            }
            /*userDictonary = new Dictionary<string, UserAC>();
            FillUserDictonary();*/
        }

        /// <summary>
        /// To fill the Dictonay with users. The key is te GUID.
        /// </summary>
        private void FillUserDictonary()
        {
            //Searching values for the AD search
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            deSearch.Filter = string.Format(@"(&(objectClass=User)(objectCategory=person)(!(title=Computer))(sn=*)(!(msExchResourceDisplay=Equipment))(!(department=Expired)))");
            deSearch.Sort.Direction = SortDirection.Ascending;
            deSearch.Sort.PropertyName = "cn";
            SearchResultCollection results = deSearch.FindAll();
            //Trace.WriteLine("-------------");
            if (!(results == null))
            {
                foreach (SearchResult qw in results)
                {
                    //Setting all values to the object and adding it to the dictonary
                    UserAC userAC = new UserAC();
                    userAC.objectGuid = new Guid((System.Byte[])qw.Properties["objectGUID"][0]).ToString();
                    userAC.userName = qw.Properties["cn"][0].ToString();


                    //Trace.WriteLine(userAC.userName + " " + userAC.objectGuid);
                    /*Trace.WriteLine("----------");
                    Trace.WriteLine(qw.Properties["cn"][0].ToString());
                    var q = 0;
                    foreach (DictionaryEntry obj in qw.Properties)
                    {
                        try
                        {

                            Trace.WriteLine(q+"-"+obj.Key.ToString()+"-"+qw.Properties[obj.Key.ToString()][0].ToString());
                        }
                        catch (Exception er) { }

                        q++;
                    }*/

                    //setting Title if it is a property
                    if (qw.Properties.Contains("title"))
                    {
                        userAC.title = qw.Properties["title"][0].ToString();
                    }

                    //setting Manager if it is a property
                    if (qw.Properties.Contains("manager"))
                    {
                        userAC.managerString = qw.Properties["manager"][0].ToString();
                    }

                    //setting department if it is a property
                    if (qw.Properties.Contains("department"))
                    {
                        userAC.department = qw.Properties["department"][0].ToString();

                        //Adding depart ment to the list if it isn't in there yet
                        if (!departmentList.Contains(userAC.department))
                        {
                            departmentList.Add(userAC.department);
                        }
                    }

                    if (qw.Properties.Contains("mail"))
                    {
                       userAC.mail = qw.Properties["mail"][0].ToString();
                       
                       //Trace.WriteLine(userAC.mail);
                       
                    }
                    userDictonary.Add(userAC.objectGuid, userAC);
                }                
            }
            SetManagers();
        }

        /// <summary>
        /// Looping over the userDictonary to connect users to their managers
        /// </summary>
        private void SetManagers()
        {
            foreach (UserAC user in userDictonary.Values)
            {
                if (user.managerString != null)
                {
                    //managerString example: CN=name of manager,OU=...
                    string managerName = user.managerString.Split(',')[0].Substring(3);

                    string managerGUID = "";

                    //Looping over userDicotnary a second time to find the object of the manager that is equal to the name.
                    foreach (UserAC maybeManager in userDictonary.Values)
                    {
                        if (maybeManager.userName.Equals(managerName))
                        {
                            managerGUID = maybeManager.objectGuid;
                            break;
                        }
                    }
                    //setting isManager for the manager and connecting it to the user
                    userDictonary[managerGUID].isManager = true;
                    user.manager = userDictonary[managerGUID];
                }
            }
        }

        /// <summary>
        /// Gives a list of users from the ActiveDirectory after filling the dictonary (if needed)
        /// </summary>
        /// <returns>List of users</returns>
        public List<UserAC> GetUsers()
        {
            FillUserDictonaryIfNeeded();

            //Sets dictonary to list
            List<UserAC> userList = userDictonary.Values.ToList();

            return userList;
        }
        
        /// <summary>
        /// Returns the departmentList after filling the dictonary (if needed)
        /// </summary>
        /// <returns>departmentList</returns>
        public List<string> GetDepartments()
        {
            FillUserDictonaryIfNeeded();

            return departmentList;
        }
        
        /// <summary>
        /// Getting users that are in the department that is given in the parameter
        /// Note: departments usally start with a capital like "Office"
        /// </summary>
        /// <param name="department"></param>
        /// <returns>A list of Users</returns>
        public List<UserAC> GetUsersByDepartment(string department)
        {
            FillUserDictonaryIfNeeded();
            List<UserAC> userList = new List<UserAC>();
            foreach (UserAC user in userDictonary.Values)
            {
                if (user.department != null && user.department.Equals(department))
                {
                    userList.Add(user);
                }
            }
            return userList;
        }

        /// <summary>
        /// Getting all users where isManager = true
        /// </summary>
        /// <returns>A list of Users</returns>
        public List<UserAC> GetManagers()
        {
            FillUserDictonaryIfNeeded();
            List<UserAC> userList = new List<UserAC>();
            foreach (UserAC user in userDictonary.Values)
            {
                if (user.isManager)
                {
                    userList.Add(user);
                }
            }
            return userList;
        }

        /// <summary>
        /// Getting all travel Agencies
        /// </summary>
        /// <returns>A list of Users</returns>
        public List<UserAC> GetTravelAgents()
        {
            FillUserDictonaryIfNeeded();
            List<UserAC> userList = new List<UserAC>();
            foreach (UserAC user in userDictonary.Values)
            {
                if (user.title != null && user.title.Equals("Travel Agent"))
                {
                    userList.Add(user);
                }
            }
            return userList;
        }

        /// <summary>
        /// Gets user by full name. upper or lower case doesn't matter
        /// If name is not found an empty UserAC object is returned with the name "UserNotFound"
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A list of Users</returns>
        public UserAC GetUserByName(string name)
        {
            FillUserDictonaryIfNeeded();
            
            foreach (UserAC user in userDictonary.Values)
            {

                if (user.userName != null && user.userName.ToLower().Equals(name.ToLower()))
                {
                    return user;
                }
            }
            return new UserAC { userName = "UserNotFound" };
        }

        /// <summary>
        /// Gets user by GUID
        /// If GUID is not found an empty UserAC object is returned with the name "UserNotFound"
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns>A list of Users</returns>
        public UserAC GetUserByGuid(string GUID)
        {
            FillUserDictonaryIfNeeded();
            if (GUID != null)
            {
                if (userDictonary.ContainsKey(GUID))
                {
                    return userDictonary[GUID];
                }
            }
            return new UserAC { userName = "UserNotFound" };
        }

        /// <summary>
        /// Get GUID based on windows user
        /// </summary>
        /// <returns></returns>
        public string GetOwnGuid()
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            deSearch.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1] + "))";
            deSearch.Sort.Direction = SortDirection.Ascending;
            deSearch.Sort.PropertyName = "cn";
            SearchResult result = deSearch.FindOne();

            return new Guid((System.Byte[])result.Properties["objectGUID"][0]).ToString();     
        }

        /// <summary>
        /// Returns UserAC object from the current user
        /// </summary>
        /// <returns></returns>
        public UserAC GetSelf()
        {
            string ownGuid = GetOwnGuid();
            return GetUserByGuid(ownGuid);
        }

        public List<UserAC> GetListOfAdUsersByGroup(string groupName)
        {
            List<UserAC> userList = new List<UserAC>();

            //Getting the domain as DirectoryEntry
            DirectoryEntry entry = GetDirectoryObject();
            DirectorySearcher search = new DirectorySearcher(entry);
            //Search query string
            string query = "(&(objectCategory=person)(objectClass=user)(memberOf=*))";
            search.Filter = query;
            search.PropertiesToLoad.Add("memberOf");
            search.PropertiesToLoad.Add("name");

            //Getting all that from the search query within the domain
            System.DirectoryServices.SearchResultCollection mySearchResultColl = search.FindAll();
            
            //Loops over all users
            foreach (SearchResult result in mySearchResultColl)
            {
                //Look over all groups of a user
                foreach (string prop in result.Properties["memberOf"])
                {
                    //If the group you search for is correct..
                    if (prop.Contains(groupName))
                    {
                        //Create new user, set variables and add it to the list
                        UserAC userAC = new UserAC();
                        userAC = GetUserByName(result.Properties["name"][0].ToString());
                        userList.Add(userAC);
                    }
                }                
            }
            //return the list
            return userList;
        }
    }
}