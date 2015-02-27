// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;

namespace Percurrentis.Model
{
    public class Accommodation
    {
        public int Id { get; set; }
        public int? AddressID { get; set; }
        public double Cost { get; set; }

        [Address]
        public virtual Address Address { get; set; }

        public int TravelRequestID { get; set; }

        public virtual TravelRequest TravelRequest { get; set; }
        
    }
}
