// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;
using System;

namespace Percurrentis.Model
{
    public class Accommodation
    {
        public int Id { get; set; }
        public int? AddressID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double? Cost { get; set; }
        public double? CostSecondary { get; set; }
        public string SecondaryCurrency { get; set; }
        public string Note { get; set; }
        public int? ParentID { get; set; }

        [Address]
        public virtual Address Address { get; set; }

        public virtual Note FeeSpecification { get; set; }

        public int? TravelRequestID { get; set; }
        public int? TravelProposalID { get; set; }

        public virtual TravelRequest TravelRequest { get; set; }
        public virtual TravelProposal TravelProposal { get; set; }
        
    }
}
