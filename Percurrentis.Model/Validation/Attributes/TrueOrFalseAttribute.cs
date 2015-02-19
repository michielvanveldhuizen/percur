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
    public class TrueOrFalseAttribute : ValidationAttribute
    {
        private const string DefaultErrormessage = "True or false must be selected";
        public TrueOrFalseAttribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
        }

        public override bool IsValid(object value)
        {
            if (value is bool)
            {
                if ((bool)value == true || (bool)value == false)
                {
                    return true;
                }
            }
            ErrorMessage = "No true or false selected for {0}";
            return false;
        }
    }
}
