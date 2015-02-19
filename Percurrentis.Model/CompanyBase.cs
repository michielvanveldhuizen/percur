// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;

namespace Percurrentis.Model
{
    public abstract class CompanyBase
    {
        public int Id { get; set; }
        [Mandatory]
        [_256]
        public string Name { get; set; }
        public int? AddressID { get; set; }

        public virtual Address Address { get; set; }
    }
}
