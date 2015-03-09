using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Percurrentis.Controllers
{
    public class TravelRequestApprovalController : ApiController
    {
        //NOT USED ATM
        [HttpGet]
        public void InitiateNotificationStep() 
        {
            Trace.WriteLine("Notification step");
        }
    }
}
