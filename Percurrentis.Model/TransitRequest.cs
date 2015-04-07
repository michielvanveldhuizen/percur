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
    public abstract class TransitRequest
    {
        public int Id { get; set; }
        [DateTime]
        public DateTime DepartureDate { get; set; }
        public int DepartureAddressID { get; set; }
        public int DestinationAddressID { get; set; }
        [Address]
        public virtual Address DepartureAddress { get; set; }
        [Address]
        public virtual Address DestinationAddress { get; set; }
        public int? TravelRequestID { get; set; }
        public int? TravelProposalID { get; set; }
        public int? ParentID { get; set; }
    }
}
