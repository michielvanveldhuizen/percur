// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;

namespace Percurrentis.Model
{
    //[Address]
    public class Address// : MetaEntity
    {
        public int Id { get; set; }
        [_256]
        public string AddressName { get; set; }
        [_256]
        public string Street { get; set; }
        [_256]
        public string City { get; set; }
        [_256]
        public string PostalCode { get; set; }
        [_256]
        public string StateProvince { get; set; }
        [_256]
        public string Longitude { get; set; }
        [_256]
        public string Latitude { get; set; }

        public int? CountryRegionID { get; set; }

        public virtual CountryInformation CountryRegion { get; set; }

        //public string AddressTypeID { get; set; }
        //[ValidateObject]
        //public AddressType AddressType { get; set; }
    }
}
