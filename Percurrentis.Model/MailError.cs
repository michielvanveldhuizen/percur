// <copyright company=CSi Romania SRL>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Michiel van Veldhuizen</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System;
using System.Collections.Generic;

namespace Percurrentis.Model
{
    public class MailError
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public string ToEmail { get; set; }
        public string TypeOfEmail { get; set; }
    }
}