// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;

namespace Percurrentis.Model
{
    public class FlyerMemberCard
    {
        public int Id { get; set; }
        [_256]
        public string FMCNumber { get; set; }
    }
}
