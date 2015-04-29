// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;
using Percurrentis.Model.Validation.Attributes;
using System;
namespace Percurrentis.Model
{
    public abstract class BaseRequest : MetaEntity
    {
        public int Id { get; set; }
        public int? ParentID { get; set; }
        public double? Cost { get; set; }
        public double? CostSecondary { get; set; }
        public string SecondaryCurrency { get; set; }
        public string Note { get; set; }

        public bool Chosen { get; set; }
        public int? CopyOf { get; set; }
    }
}
