// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;
using Percurrentis.Model.Validation.Attributes;
using System;

namespace Percurrentis.Model
{
    public class RentalCarRequest
    {
        public int Id { get; set; }
        [DateTime]
        public DateTime StartDate { get; set; }
        [DateTime]
        public DateTime EndDate { get; set; }
        public int DriverID { get; set; }
        public int? SecondaryDriverID { get; set; }
        public int AddressID { get; set; }
        public int? TravelRequestID { get; set; }
        public int? TravelProposalID { get; set; }
        public int? ServiceCompanyID { get; set; }
        [ValidateObject]
        public virtual RequestTraveller Driver { get; set; }
        [ValidateObject]
        public virtual RequestTraveller SecondaryDriver { get; set; }
        [Address]
        public Address Address { get; set; }
        public virtual TravelRequest TravelRequest { get; set; }
        public virtual TravelProposal TravelProposal { get; set; }
        public virtual ServiceCompany ServiceCompany { get; set; }
        public double? Cost { get; set; }
        public double? CostSecondary { get; set; }
        public string SecondaryCurrency { get; set; }
        public string Note { get; set; }
    }
}
