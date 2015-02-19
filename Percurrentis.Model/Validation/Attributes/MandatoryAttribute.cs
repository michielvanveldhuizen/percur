// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Validation classes for the database</summary>

using System;
using System.ComponentModel.DataAnnotations;

namespace Percurrentis.Model.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MandatoryAttribute : ValidationAttribute
    {
        private const string DefaultErrormessage = "{0} is a mandatory attribute";
        public MandatoryAttribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                  return false;
            }
            return true;
        }
    }
}
