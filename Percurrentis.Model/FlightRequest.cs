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
    public class FlightRequest
    {
        public int Id { get; set; }
        [DateTime]
        public DateTime DepartureDate { get; set; }
        [TrueOrFalse]
        public bool HasLargeCabinLuggage { get; set; }
        [TrueOrFalse]
        public bool HasSpecialEquipment { get; set; }
        //only required if HasSpecialEquipment is true
        public int? LargeLuggageCount { get; set; }
        [TrueOrFalse]
        public bool IsOnlineCheckIn { get; set; }
        public int? FlyerMemberCardID { get; set; }
        public int DepartureAddressID { get; set; }
        public int DestinationAddressID { get; set; }
        //not required due to lack of implementation
        public string Airline { get; set; }
        public int? TravelRequestID { get; set; }
        public int? TravelProposalID { get; set; }
        public int? ParentID { get; set; }
        public double? Cost { get; set; }
        public double? CostSecondary { get; set; }
        public string SecondaryCurrency { get; set; }
        public string Note { get; set; }

        public virtual FlyerMemberCard FlyerMemberCard { get; set; }

        public virtual AirportInformation DepartureAddress { get; set; }
        public virtual AirportInformation DestinationAddress { get; set; }
        //[ValidateObject]
        //public ServiceCompany ServiceCompany { get; set; }

        public virtual TravelRequest TravelRequest { get; set; }
        public virtual TravelProposal TravelProposal { get; set; }
    }
}
