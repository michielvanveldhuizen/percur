// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;
using Percurrentis.Model.Validation.Attributes;
namespace Percurrentis.Model
{
    public class ContactPerson
    {
        public int Id { get; set; }
        [Mandatory]
        [_256]
        public string Name { get; set; }
        [_256]
        public string EmailAddress { get; set; }
        public int PhoneNumberID { get; set; }
        [ValidateObject]
        public virtual PhoneNumber PhoneNumber { get; set; }
    }
}
