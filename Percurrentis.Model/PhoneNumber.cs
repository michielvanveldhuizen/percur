// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;

namespace Percurrentis.Model
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public int? CountryInformationID { get; set; }
        [Mandatory]
        [_256]
        public string Number { get; set; }
        public CountryInformation CountryInformation { get; set; }
    }
}
