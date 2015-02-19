// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    public class UserAC
    {
        public string objectGuid { get; set; }
        public string userName { get; set; }
        public string managerString { get; set; }
        public UserAC manager { get; set; }
        public bool isManager { get; set; }
        public string title { get; set; }
        public string department { get; set; }
        public string mail { get; set; }
    }
}
