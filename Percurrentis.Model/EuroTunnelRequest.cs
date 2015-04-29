// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System;
namespace Percurrentis.Model
{
    public class EuroTunnelRequest : BaseRequest
    {
        [_256]
        public string LicensePlate { get; set; }

        [DateTime]
        public DateTime DepartureDate { get; set; }
        public int DepartureAddressID { get; set; }
        public int DestinationAddressID { get; set; }

        public virtual Address DepartureAddress { get; set; }
        public virtual Address DestinationAddress { get; set; }

        public int? TravelRequestID { get; set; }
        public int? TravelProposalID { get; set; }
        public virtual TravelRequest TravelRequest { get; set; }
        public virtual TravelProposal TravelProposal { get; set; }

    }
}
