// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System;
using System.Collections.Generic;

namespace Percurrentis.Model
{
    public class RequestTraveller : MetaEntity
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
        public string TravelDocument { get; set; }

        public string Department { get; set; }

        public int? CompanyID { get; set; }
        //public string FlyerMemberCardID { get; set; }
        //[ValidateObject]
        //public PhoneNumber PhoneNumber { get; set; }
        public virtual Company Company { get; set; }
        //[ValidateObject]
        //public FlyerMemberCard FlyerMemberCard { get; set; }

        public string PassportNumber { get; set; }
        public DateTime PassportExpiryDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }

        public string Note { get; set; }

        public int? AddressID { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<TravelRequest_RequestTraveller> TravelRequest_RequestTravellers { get; set; }
    }
}
