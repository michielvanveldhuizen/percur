// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

namespace Percurrentis.Model
{
    public class TaxiRequest : TransitRequest
    {
        public virtual TravelRequest TravelRequest { get; set; }
        public virtual TravelProposal TravelProposal { get; set; }
        public double? Cost { get; set; }
        public double? CostSecondary { get; set; }
        public string SecondaryCurrency { get; set; }
        public string Note { get; set; }

    }
}
