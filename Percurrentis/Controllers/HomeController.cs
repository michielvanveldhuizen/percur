using System;
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

namespace Percurrentis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ADservices AD = ADservices.InstanceCreation();
            UserAC self = AD.GetSelf();

            AccessLevels accessLevels = SetAccessLevels(self);

            ControllerData cd = new ControllerData { Name = self.userName, Guid = self.objectGuid, AccessLevels = accessLevels};

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

            if (user.isManager)
            {
                AL.ProjectManager = true;
            }

            //Not sure if every Financial is accountant
            if (user.department != null && user.department.Equals("Financial"))
            {
                AL.Accountant = true;
            }

            //TODO AL OF THESE BUT NOT SURE WHERE TO FIND INFORMATION IN AD YET
            //AL.supervisor = true;
            //AL.Accountant = true;
            //AL.COO = true;

            return AL;
        }
    }

    public class ControllerData
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public AccessLevels AccessLevels { get; set; }
    }
}