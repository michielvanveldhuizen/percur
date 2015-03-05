// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
namespace Percurrentis.Model
{
    public class EuroTunnelRequest : TransitRequest
    {
        [_256]
        public string LicensePlate { get; set; }
        public virtual TravelRequest TravelRequest { get; set; }
        public double? Cost { get; set; }

    }
}
