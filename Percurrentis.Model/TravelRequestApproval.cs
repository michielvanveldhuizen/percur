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
    public class TravelRequestApproval
    {
        public int Id { get; set; }
        public string ApprovedBy { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public bool Archived { get; set; }
        public bool Flag { get; set; }
        public int HasApproved { get; set; }
        public bool NotificationSent { get; set; }
        public int? ApprovalRoleID { get; set; }
        public int? NextID { get; set; }
        public string Hash { get; set; }

        public virtual ApprovalRole ApprovalRole { get; set; }
        public virtual TravelRequestApproval Next { get; set; }
    
    }
}
