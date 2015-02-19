// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    [Forbidden]
    public class AirportInformation
    {
        [Forbidden]
        public int Id { get; set; }
        [Forbidden]
        public string Name { get; set; }
        [Forbidden]
        public string City { get; set; }
        [Forbidden]
        public string Country { get; set; }
        [Forbidden]
        public string IATA_FAA { get; set; }
        [Forbidden]
        public string ICAO { get; set; }
        [Forbidden]
        public string Latitude { get; set; }
        [Forbidden]
        public string Longitude { get; set; }
        [Forbidden]
        public string Altitude { get; set; }
        [Forbidden]
        public string Timezone { get; set; }
        [Forbidden]
        public string DST { get; set; }       
    }
}
