// <copyright company=CSi Romania SRL>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Peter Feddema</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System;
using System.Collections.Generic;

namespace Percurrentis.Model
{
    public class Insurance
    {
        public int Id { get; set; }
        [_256]
        public string InsureeGUID { get; set; }
        [DateTime]
        public DateTime StartDate { get; set; }
        [DateTime]
        public DateTime ExpirationDate { get; set; }
        public string InsuranceNumber { get; set; }
    }
}
