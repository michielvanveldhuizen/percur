// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;

namespace Percurrentis.Model
{
    [Forbidden]
    public class CountryInformation
    {
        [Forbidden]
        public int Id { get; set; }
        [Forbidden]
        public string Name { get; set; }
        [Forbidden]
        public string CountryCode { get; set; }
        [Forbidden]
        public string ISO { get; set; }
        [Forbidden]
        public string CallCode { get; set; }
    } 
}
