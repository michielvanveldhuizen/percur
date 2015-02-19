// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Main class for the Windows Service</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TravelRequestApproval
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the service.
        /// </summary>
        static void Main()
        {
#if(!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new TRAservice() 
            };
            ServiceBase.Run(ServicesToRun);
#else
            TRAservice Service = new TRAservice();
            Service.CanHandlePowerEvent = false;
            Service.CanHandleSessionChangeEvent = false;
            Service.CanPauseAndContinue = true;
            Service.CanShutdown = true;
            Service.CanStop = true;
            Service.ServiceName = "TRAservice";
            Service.AutoLog = false;
            Service.SetMethod();
            Service.TRAmethod();
#endif
        }
    }
}