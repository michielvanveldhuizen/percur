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
            //Using the ADservice as singleTon
            ADservices AD = ADservices.InstanceCreation();
            UserAC self = AD.GetSelf();

            AccessLevels accessLevels = SetAccessLevels(self);

            //Setting the ControllerData for the view
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

            //For testing-------------//
            //AL.TravelAgency = true;
            //-----------------------//

            //Probably this will be removed
            if (user.isManager)
            {
                AL.ProjectManager = true;
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

    public class ControllerData
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public AccessLevels AccessLevels { get; set; }
    }
}