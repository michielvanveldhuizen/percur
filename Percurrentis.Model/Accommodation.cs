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
        public double? DailyRate { get; set; }
        public double? AdditionalFees { get; set; }

        [Address]
        public virtual Address Address { get; set; }

        public virtual Note FeeSpecification { get; set; }

        public int TravelRequestID { get; set; }

        public virtual TravelRequest TravelRequest { get; set; }
        
    }
}
