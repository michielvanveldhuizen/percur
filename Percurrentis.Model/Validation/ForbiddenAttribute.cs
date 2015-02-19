// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Validation classes for the database</summary>

using System.ComponentModel.DataAnnotations;

namespace Percurrentis.Model.Validation
{
    public class ForbiddenAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return new ValidationResult("Not allowed to add data to this table");
        }
    }
}
