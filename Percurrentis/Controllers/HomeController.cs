﻿using System;
using System.DirectoryServices.AccountManagement;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web;
using System.DirectoryServices;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Percurrentis.AD_classes;
using System.Security.Principal;
using Percurrentis.Model;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Percurrentis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Using the ADservice as singleton
            ADservices AD = ADservices.InstanceCreation();
            
            UserAC self = AD.GetSelf();


            //List<UserAC> dingen = AD.GetListOfAdUsersByGroup("Travel Approval");

            //ToTest if mail works
            //NotificationCenter.Notification.notifyOfDenial(self, new TravelRequest());
            
            AccessLevels accessLevels = SetAccessLevels(self);     

            //devTools Mode to overwrite the accessLevels
            HttpCookie devTools = Request.Cookies["devTools"];
            if (GlobalVar.developMode)
            {
                if (devTools != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    JsonObject jsonObject = serializer.Deserialize<JsonObject>(devTools.Value.ToString());

                    accessLevels.Accountant = jsonObject.Accountant;
                    accessLevels.TravelAgency = jsonObject.TravelAgency;
                    accessLevels.COO = jsonObject.COO;
                    if (accessLevels.COO == true)
                    {
                        self.objectGuid = GlobalVar.COOGuid;
                    }
                    accessLevels.ProjectManager = jsonObject.AlwaysManager;
                }
            }


            //Setting the ControllerData for the view
            ControllerData cd = new ControllerData { Name = self.userName, Guid = self.objectGuid, AccessLevels = accessLevels,DevelopMode=GlobalVar.developMode};

            using (HostingEnvironment.Impersonate())
            {
                return View(cd);
            }
        }

        public static AccessLevels SetAccessLevels(UserAC user)
        {
            AccessLevels AL = new AccessLevels();
            AL.Employee = true;

            if (user.department != null && user.department.Equals("Travel Agent"))
            {
                AL.TravelAgency = true;
            }

            ADservices AD = ADservices.InstanceCreation();

            List<UserAC> approvalList = AD.GetListOfAdUsersByGroup("Travel Approval");

            foreach (UserAC u in approvalList)
            {
                if (u.userName.Equals(user.userName))
                {
                    AL.ProjectManager = true;
                }
            }

            //Not sure if every Financial is accountant
            if (user.department != null && user.department.Equals("Financial"))
            {
                AL.Accountant = true;
            }

            if (user.objectGuid.Equals("a73d1a5e-b640-467e-8583-e4b52cfae437"))
            {
                AL.COO = true;
            }

            return AL;
        }
    }

    //TO send to the view
    public class ControllerData
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public AccessLevels AccessLevels { get; set; }
        public bool DevelopMode { get; set; }
    }

    //To load the cookie as JSONobject
    public class JsonObject
    {
        public bool TravelAgency { get; set; }
        public bool COO { get; set; }
        public bool Accountant { get; set; }
        public bool AlwaysManager { get; set; }
    }
}