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
    public class CannotContainNumbersAttribute : ValidationAttribute
    {
        private const string _pattern = @"^((?!(?:[0-9])).)*$";
        private const string DefaultErrormessage = "{0} cannot contain numbers";
        public CannotContainNumbersAttribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }

        public string Pattern
        {
            get { return _pattern; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var regexValidator = new RegularExpressionAttribute(_pattern);
                if (!regexValidator.IsValid(value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
