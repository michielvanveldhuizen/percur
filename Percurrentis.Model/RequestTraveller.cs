// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System.Collections.Generic;

namespace Percurrentis.Model
{
    public class RequestTraveller
    {
        public int Id { get; set; }
        [Mandatory]
        [_256]
        public string FamilyName { get; set; }
        [Mandatory]
        [_256]
        public string FirstName { get; set; }
        [Mandatory]
        [_256]
        public string FullName { get; set; }
        public string PhoneNumberID { get; set; }
        public int? CompanyID { get; set; }
        //public string FlyerMemberCardID { get; set; }
        public int TravelRequestID { get; set; }
        //[ValidateObject]
        //public PhoneNumber PhoneNumber { get; set; }
        public virtual Company Company { get; set; }
        //[ValidateObject]
        //public FlyerMemberCard FlyerMemberCard { get; set; }
        
        public virtual ICollection<TravelRequest> TravelRequests { get; set; }
    }
}
