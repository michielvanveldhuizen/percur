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
    }
}
