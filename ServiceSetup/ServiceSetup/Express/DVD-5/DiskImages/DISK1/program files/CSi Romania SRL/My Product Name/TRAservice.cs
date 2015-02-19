// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Code for the Windows Service with all its methods</summary>

using Percurrentis.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using System.Net.Mail;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.ServiceProcess;
using System.Timers;

namespace TravelRequestApproval
{
    public partial class TRAservice : ServiceBase
    {
        private Timer scheduleTimer = null;
        private bool flag;
        private string conString = ConfigurationManager.ConnectionStrings["PercurrentisContext"].ConnectionString;

        public TRAservice()
        {
            InitializeComponent();

            CanHandlePowerEvent = false;
            CanHandleSessionChangeEvent = false;
            CanPauseAndContinue = true;
            CanShutdown = true;
            CanStop = true;
            AutoLog = false;

            scheduleTimer = new Timer();
            scheduleTimer.Interval = 60000;
            scheduleTimer.Elapsed += new ElapsedEventHandler(scheduleTimer_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";

            ((ISupportInitialize)(this.EventLog)).BeginInit();
            if (!EventLog.SourceExists(this.EventLog.Source))
            {
                EventLog.CreateEventSource(this.EventLog.Source, this.EventLog.Log);
            }
            ((ISupportInitialize)(this.EventLog)).EndInit();

            EventLog.WriteEntry("Service started", EventLogEntryType.Information, 1, 100);
            flag = true;
            scheduleTimer.Start();
        }

        protected void scheduleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (flag == true)
            {
                TRAmethod();
                flag = false;
            }
            else
            {
                SetMethod();
                flag = true;
            }
        }

        public void SetMethod()
        {
            EventLog.WriteEntry("Retrieving travel requests", EventLogEntryType.Information, 1, 2);
            //declarations
            DataSet ds = new DataSet();
            DataTable dt, dt3, dt4 = new DataTable();
            string query, query2, query3, query4, query5;
            object[] objArr;
            int i = 0;
            int? p;
            bool isSet = false;
            Percurrentis.Model.TravelRequestApproval TRAnext, TRAnew;


            //start logic
            query = @"SELECT *
            FROM TravelRequest";
            dt = ExecuteQuery(query);
            foreach (DataRow fdr1 in dt.Rows)
            {
                if (fdr1["TravelRequestApprovalID"].Equals(DBNull.Value))
                {
                    EventLog.WriteEntry("Adding TRA logic to TR", EventLogEntryType.Information, 2, 2);
                    query2 = @"SELECT count(*) 
                    FROM Company 
                    INNER JOIN RequestTraveller
                    ON Company.Id = RequestTraveller.CompanyID
                    INNER JOIN TravelRequest
                    ON RequestTraveller.TravelRequestID = " + fdr1["Id"];
                    //check if traveler works at CSi Industries
                    if (YesQuery(query2).Equals(1))
                    {
                        query3 = @"SELECT count(*)
                        FROM CountryInformation ci 
                        INNER JOIN TravelRequest tr
                        ON tr.CountryID = ci.Id
                        WHERE tr.Id = " + fdr1["Id"] +
                        "AND ci.Id = 173";
                        //check if the destination is romania
                        if (YesQuery(query3).Equals(1))
                        {
                            EventLog.WriteEntry("Adding COO TRA logic to TR", EventLogEntryType.Information, 3, 2);
                            TRAnext = new Percurrentis.Model.TravelRequestApproval();
                            TRAnext.ApprovalRoleID = 3;
                            TRAnext.TravelRequestID = (int)fdr1["Id"];
                            TRAnext.Hash = String.Format("{0:X}", DateTime.Now.GetHashCode());
                            objArr = new object[] { TRAnext };
                            i = InsertQuery(objArr);
                            EventLog.WriteEntry("Rows added to TRA: " + i, EventLogEntryType.Information, 4, 2);
                            isSet = true;
                        }
                    }
                    TRAnew = new Percurrentis.Model.TravelRequestApproval();
                    TRAnew.ApprovalRoleID = 1;
                    TRAnew.TravelRequestID = (int)fdr1["Id"];
                    TRAnew.Hash = String.Format("{0:X}", DateTime.Now.GetHashCode());
                    Debug.WriteLine(TRAnew.TravelRequestID + " +  " + TRAnew.Hash);
                    if (isSet == true)
                    {
                        query4 = @"SELECT TOP 1 * 
                        FROM TravelRequestApproval
                        WHERE TravelRequestId = " + fdr1["Id"] + @"
                        AND ApprovalRoleId = 3";
                        dt3 = ExecuteQuery(query4);
                        TRAnew.NextID = (int)dt3.Rows[0]["Id"];
                    }
                    else
                    {
                        TRAnew.NextID = null;
                    }
                    objArr = new object[] { TRAnew };
                    i = InsertQuery(objArr);
                    EventLog.WriteEntry("Rows added to TRA: " + i, EventLogEntryType.Information, 5, 2);
                    query5 = @"SELECT TOP 1 * 
                        FROM TravelRequestApproval
                        WHERE TravelRequestId = " + fdr1["Id"] + @"
                        AND ApprovalRoleId = 1";
                    dt4 = ExecuteQuery(query5);
                    p = (int)dt4.Rows[0]["Id"];
                    objArr = new object[] { false, p, fdr1["Id"] };
                    i = UpdateQuery(objArr);
                    EventLog.WriteEntry("Rows added to TR: " + i, EventLogEntryType.Information, 6, 2);

                }
                else if (fdr1["IsApproved"].Equals(true))
                {
                    string q = @"SELECT TOP 1 * 
                            FROM TravelRequestApproval
                            WHERE Id = " + fdr1["TravelRequestApprovalID"];
                    DataTable dtx = ExecuteQuery(q);
                    MailServices m = new MailServices();
                    m.MailMethod("TravelAgency@csiweb.ro", "Travel Agency", null, null);
                }
            }
        }

        public void TRAmethod()
        {
            EventLog.WriteEntry("Retrieving status of approval process", EventLogEntryType.Information, 1, 3);
            int i = 0;
            DataSet ds = new DataSet();
            DataTable dt, dt3 = new DataTable();
            string query = @"SELECT *
            FROM TravelRequestApproval";
            dt = ExecuteQuery(query);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Flag"].Equals(true))
                {
                    if (dr["HasApproved"].Equals(true))
                    {
                        if (dr["NextId"].Equals(DBNull.Value))
                        {
                            string q = @"SELECT TOP 1 * 
                            FROM TravelRequest
                            WHERE TravelRequestApprovalId = " + dr["Id"];
                            DataTable dtx = ExecuteQuery(q);
                            i = UpdateQuery(new object[] { true, dr["Id"], (int)dtx.Rows[0]["Id"] });
                            EventLog.WriteEntry("Rows updated in TR: " + i, EventLogEntryType.Information, 2, 3);
                            i = UpdateTRA(new object[] { dr["ApprovedBy"], DateTime.Now, dr["Archived"], false, true, true, dr["Id"], dr["Hash"] });
                            EventLog.WriteEntry("Rows updated in TRA: " + i, EventLogEntryType.Information, 3, 3);
                            //FINALIZE, APPROVED - notify parties
                        }
                        else
                        {
                            string q = @"SELECT TOP 1 * 
                            FROM TravelRequestApproval
                            WHERE TravelRequestId = " + dr["TravelRequestId"] + @"
                            AND ApprovalRoleId = 3";
                            DataTable dtx = ExecuteQuery(q);
                            i = UpdateQuery(new object[] { false, (int)dtx.Rows[0]["Id"], dr["TravelRequestId"] });
                            EventLog.WriteEntry("Rows updated in TR: " + i, EventLogEntryType.Information, 4, 3);
                            i = UpdateTRA(new object[] { dr["ApprovedBy"], DateTime.Now, dr["Archived"], false, true, true, dr["Id"], dr["Hash"] });
                            EventLog.WriteEntry("Rows updated in TRA: " + i, EventLogEntryType.Information, 5, 3);
                            //next step
                            //get info from dr["next"]
                        }
                    }
                    else
                    {
                        i = UpdateTRA(new object[] { dr["ApprovedBy"], DateTime.Now, true, false, false, true, dr["Id"], dr["Hash"] });
                        EventLog.WriteEntry("Rows updated in TRA: " + i, EventLogEntryType.Information, 6, 3);
                        //BREAK - NO APPROVAL, notify approval is denied           
                        string qt = @"SELECT TOP 1 * 
                            FROM TravelRequest
                            WHERE TravelRequestApprovalId = " + dr["Id"];
                        DataTable dtx = ExecuteQuery(qt);
                        int no = (int)dtx.Rows[0]["Id"];
                        MailServices m = new MailServices();
                        m.MailMethod("TravelAgency@csiweb.ro", "Travel Agency", no.ToString(), "1");
                    }
                }
                else
                {
                    if (dr["NotificationSent"].Equals(false))
                    {
                        string q = @"SELECT TOP 1 * 
                            FROM TravelRequest
                            WHERE TravelRequestApprovalId = " + dr["Id"];
                        DataTable dtx = ExecuteQuery(q);
                        int no = (int)dtx.Rows[0]["Id"];
                        string hash = dr["Hash"].ToString();
                        MailServices m = new MailServices();
                        if (ADServices.GetGuidFromDatabase(no).Equals(""))
                        {
                            m.MailMethod("TravelAgency@csiweb.ro", "Travel Agency", no.ToString(), hash);
                        }
                        else
                        {
                            List<String> mgrDetails = ADServices.getManagerDetails(ADServices.Guid2OctetString(ADServices.GetGuidFromDatabase(no)));
                            if (mgrDetails[1] == null)
                            {
                                m.MailMethod("TravelAgency@csiweb.ro", "Travel Agency", no.ToString(), hash);
                            }
                            else
                            {
                                m.MailMethod(mgrDetails[0], mgrDetails[1], no.ToString(), hash);
                            }
                        }
                        i = UpdateTRA(new object[] { dr["ApprovedBy"], dr["ApprovalDate"], dr["Archived"], dr["Flag"], dr["HasApproved"], true, dr["Id"], dr["Hash"] });
                        EventLog.WriteEntry("Rows updated in TRA: " + i, EventLogEntryType.Information, 7, 3);
                    }
                    else
                    {
                        //This will be the mostly selected outcome of this method. However, this means nothing will happen.
                        //For you, my fellow devloper: ส็_(ツ)_ส้้้ Thumbs up!
                    }
                }
            }
        }

        private int InsertQuery(object[] param)
        {
            EventLog.WriteEntry("In InsertQuery method ", EventLogEntryType.Information, 1, 10);
            int s = 0;
            foreach (object obj in param)
            {
                if (!(obj is Percurrentis.Model.TravelRequestApproval))
                {
                    EventLog.WriteEntry("Wrong objects inserted in InsertQuery method ", EventLogEntryType.Error, 43, 4);
                    return -1;
                }
                var insObj = obj as Percurrentis.Model.TravelRequestApproval;
                using (var connection = new SqlConnection(conString))
                {
                    string saveTRA = "INSERT into TravelRequestApproval (ApprovalRoleID,TravelRequestID,NextID,Archived,Flag,HasApproved,NotificationSent,Hash) VALUES (@ApprovalRoleID,@TravelRequestID,@NextID,@Archived,@Flag,@HasApproved,@NotificationSent,@Hash)";
                    using (SqlCommand querySaveTRA = new SqlCommand(saveTRA))
                    {
                        querySaveTRA.Connection = connection;
                        connection.Open();
                        querySaveTRA.Parameters.AddWithValue("@ApprovalRoleID", insObj.ApprovalRoleID);
                        querySaveTRA.Parameters.AddWithValue("@TravelRequestID", insObj.TravelRequestID);
                        querySaveTRA.Parameters.AddWithValue("@NextID", insObj.NextID == null ? DBNull.Value : (object)insObj.NextID);
                        querySaveTRA.Parameters.AddWithValue("@Archived", false);
                        querySaveTRA.Parameters.AddWithValue("@Flag", false);
                        querySaveTRA.Parameters.AddWithValue("@HasApproved", false);
                        querySaveTRA.Parameters.AddWithValue("@NotificationSent", false);
                        querySaveTRA.Parameters.AddWithValue("@Hash", insObj.Hash);

                        s += querySaveTRA.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            EventLog.WriteEntry("Leaving InsertQuery method, see next event for information", EventLogEntryType.Information, 2, 10);
            return s;
        }

        private int UpdateQuery(object[] param)
        {
            EventLog.WriteEntry("In UpdateQuery method ", EventLogEntryType.Information, 1, 11);
            int s = 0;
            using (var connection = new SqlConnection(conString))
            {
                string updateTR = "UPDATE TravelRequest SET IsApproved = @IsApproved, TravelRequestApprovalID = @TravelRequestApprovalID WHERE Id = @Id";

                using (SqlCommand queryUpdateTR = new SqlCommand(updateTR))
                {
                    queryUpdateTR.Connection = connection;
                    connection.Open();
                    queryUpdateTR.Parameters.AddWithValue("@IsApproved", param[0]);
                    queryUpdateTR.Parameters.AddWithValue("@TravelRequestApprovalID", param[1]);
                    queryUpdateTR.Parameters.AddWithValue("@Id", param[2]);

                    s += queryUpdateTR.ExecuteNonQuery();
                    connection.Close();
                }
            }
            EventLog.WriteEntry("Leaving UpdateQuery method, see next event for information", EventLogEntryType.Information, 2, 11);
            return s;
        }

        //input object should have the following layout
        //obj.nr    =   field
        //0         =   ApprovedBy
        //1         =   ApprovalDate
        //2         =   Archived
        //3         =   Flag
        //4         =   HasApproved
        //5         =   NotificationSent
        //6         =   Id (to support WHERE clausule)
        //7         =   Hash
        private int UpdateTRA(object[] param)
        {
            EventLog.WriteEntry("In UpdateTRA method ", EventLogEntryType.Information, 1, 12);
            int s = 0;
            using (var connection = new SqlConnection(conString))
            {
                string updateTR = "UPDATE TravelRequestApproval SET ApprovedBy = @ApprovedBy, ApprovalDate = @ApprovalDate, Archived = @Archived, Flag = @Flag, HasApproved = @HasApproved, NotificationSent = @NotificationSent, Hash = @Hash WHERE Id = @Id";

                using (SqlCommand queryUpdateTR = new SqlCommand(updateTR))
                {
                    queryUpdateTR.Connection = connection;
                    connection.Open();
                    queryUpdateTR.Parameters.AddWithValue("@ApprovedBy", param[0]);
                    queryUpdateTR.Parameters.AddWithValue("@ApprovalDate", param[1]);
                    queryUpdateTR.Parameters.AddWithValue("@Archived", param[2]);
                    queryUpdateTR.Parameters.AddWithValue("@Flag", param[3]);
                    queryUpdateTR.Parameters.AddWithValue("@HasApproved", param[4]);
                    queryUpdateTR.Parameters.AddWithValue("@NotificationSent", param[5]);
                    queryUpdateTR.Parameters.AddWithValue("@Id", param[6]);
                    queryUpdateTR.Parameters.AddWithValue("@Hash", param[7]);

                    s += queryUpdateTR.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return s;
        }

        private int YesQuery(string query)
        {
            EventLog.WriteEntry("In YesQuery method ", EventLogEntryType.Information, 1, 13);
            try
            {
                using (var connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    EventLog.WriteEntry("Leaving ExecuteQuery method ", EventLogEntryType.Information, 2, 13);
                    return count;
                }
            }
            catch (SqlException e)
            {
                EventLog.WriteEntry("SQLconnection failed: " + e.Message, EventLogEntryType.Error, 3, 13);
                return 0;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("An exception occured while connecting the database (unrelated to SQLconnection): " + e.Message, EventLogEntryType.Error, 4, 13);
                return 0;
            }
        }
        private DataTable ExecuteQuery(string query)
        {
            EventLog.WriteEntry("In ExecuteQuery method ", EventLogEntryType.Information, 1, 13);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(conString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(ds, "TRA");
                    dt = ds.Tables["TRA"];
                    EventLog.WriteEntry("Leaving ExecuteQuery method ", EventLogEntryType.Information, 2, 13);
                    return dt;
                }
            }
            catch (SqlException e)
            {
                EventLog.WriteEntry("SQLconnection failed: " + e.Message, EventLogEntryType.Error, 3, 13);
                return null;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("An exception occured while connecting the database (unrelated to SQLconnection): " + e.Message, EventLogEntryType.Error, 4, 13);
                return null;
            }
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Service stopped", EventLogEntryType.Information, 2, 100);
            scheduleTimer.Stop();
            EventLog.WriteEntry("Stopped");
        }
        protected override void OnPause()
        {
            EventLog.WriteEntry("Service paused", EventLogEntryType.Information, 3, 100);
            scheduleTimer.Stop();
            EventLog.WriteEntry("Paused");
        }
        protected override void OnContinue()
        {
            EventLog.WriteEntry("Service continued", EventLogEntryType.Information, 4, 100);
            scheduleTimer.Start(); ;
            EventLog.WriteEntry("Continuing");
        }
        protected override void OnShutdown()
        {
            EventLog.WriteEntry("Service shutdown", EventLogEntryType.Information, 5, 100);
            scheduleTimer.Stop();
            EventLog.WriteEntry("ShutDowned");
        }
    }

    public static class ADServices
    {
        private static DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry oDE;
            oDE = new DirectoryEntry("LDAP://OU=Romania,DC=CSiRO,DC=local");
            return oDE;
        }

        public static List<String> getManagerDetails(string input)
        {
            List<UserAC> arra = new List<UserAC>();
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            Debug.WriteLine(input);
            deSearch.Filter = string.Format(@"(&(objectClass=User)(objectCategory=person)(objectGuid=" + input + "))");
            deSearch.Sort.Direction = SortDirection.Ascending;
            deSearch.Sort.PropertyName = "cn";
            SearchResultCollection results = deSearch.FindAll();
            if (!(results == null))
            {
                foreach (SearchResult qw in results)
                {
                    return new List<String> { qw.Properties["mail"][0].ToString(), qw.Properties["cn"][0].ToString() };
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public static string GetGuidFromDatabase(int id)
        {
            string conString = ConfigurationManager.ConnectionStrings["PercurrentisContext"].ConnectionString;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            var query = @"Select SuperiorID
                FROM TravelRequest
                WHERE Id = " + id;
            try
            {
                using (var connection = new SqlConnection(conString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(ds, "t");
                    dt = ds.Tables["t"];
                }
                Debug.WriteLine(dt.Rows[0]["SuperiorID"]);
                return dt.Rows[0]["SuperiorID"].ToString();
            }
            catch (SqlException e)
            {
                Debug.WriteLine("SQLconnection failed: " + e.Message);
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("An exception occured while connecting the database (unrelated to SQLconnection): " + e.Message);
                return null;
            }
        }

        public static string Guid2OctetString(string objectGuid)
        {
            System.Guid guid = new Guid(objectGuid);
            byte[] byteGuid = guid.ToByteArray();
            string queryGuid = "";
            foreach (byte b in byteGuid)
            {
                queryGuid += @"\" + b.ToString("x2");
            }
            Debug.WriteLine(queryGuid);
            return queryGuid;
        }

        public static SearchResultCollection getUser(string name)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = string.Format(@"(&(objectClass=User)(objectCategory=person)(name=" + name + "))");
            deSearch.Sort.Direction = SortDirection.Ascending;
            deSearch.Sort.PropertyName = "cn";
            return deSearch.FindAll();
        }
    }
    public class MailServices
    {
        //needs functional implementation, constructor needs to be made suitable for re-use of method 
        public void MailMethod(string email, string name, string id, string hash)
        {
            SmtpClient s = new SmtpClient("MAILRO.CSiRO.local");
            s.UseDefaultCredentials = false;
            ///////////////////////////////////////
            // EDIT THE ROW BELOW IN ORDER TO GET THE E-MAL FUNCTAIONALITY WORKING
            ///////////////////////////////////////
            s.Credentials = new NetworkCredential("USER", "password");
            s.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage m = new MailMessage("TravelAgency@csiweb.ro", email);
            if (hash.Equals(null))
            {
                m.Subject = "Approved Travel Request";
                m.Body = "Dear Travel Agency,\n" +
                    "\n" +
                    "A Travel Request is approved.\n" +
                    "You may now start making reservations and ordering tickets for this trip .\n" +
                    "The attached link leads to an overview page with Travel Requests.\n" +
                    "Select 'Approved' from the filter in the right top to show approved requests only.\n" +
                    "\n" +
                    "http://appro.csiro.local/TravelAgency/#/Request/" +
                    "\n" +
                    "\n" +
                    "This email is automatically generated. Please do not respond. For further inquiries, please contact the Travel Agency. \n";
            }
            else if (hash.Equals("1"))
            {
                m.Subject = "Denied Travel Request";
                m.Body = "Dear Travel Agency,\n" +
                    "\n" +
                    "A Travel Request is denied.\n" +
                    "The request is automatically archived.\n" +
                    "\n" +
                    "http://appro.csiro.local/#/Request/" + id +
                    "\n" +
                    "\n" +
                    "This email is automatically generated. Please do not respond. For further inquiries, please contact the Travel Agency. \n";
            }
            else
            {
                m.Subject = "Travel Request Approval";
                m.Body = "Dear " + name + ",\n" +
                    "\n" +
                    "There is a new Travel Request that needs your permission.\n" +
                    "Please follow the link as attached to this email in order to approve this request.\n" +
                    "\n" +
                    "http://appro.csiro.local/#/Request/" + id + "?approve=" + hash + " \n" +
                    "\n" +
                    "\n" +
                    "\n" +
                    "This email is automatically generated. Please do not respond. For further inquiries, please contact the Travel Agency. \n";
            }
            try
            {
                m.IsBodyHtml = true;
                s.Send(m);
                Debug.Write(m.Body);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message + "/n" + ex.InnerException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
