// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Validation classes for the database</summary>

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Percurrentis.Model.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Decimal62Attribute : ValidationAttribute
    {

        private const string DefaultErrormessage = "{0} is a not a valid (6 numbers of which 2 precision values) attribute";
        public Decimal62Attribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
        }

        public override bool IsValid(object value)
        {
            //it is not up to this validator to judge on an empty value
            if ((value == null) || (value.GetType() == typeof(Decimal)))
            {
                var dec = (Decimal)value;
                if (dec < 0.01M)
                {
                    ErrorMessage = "The decimal value cannot be smaller or equal to zero";
                    return false;
                }
                int countVal = Math.Truncate(Math.Abs(dec)).ToString(CultureInfo.InvariantCulture).Length;
                if (countVal > 4)
                {
                    ErrorMessage = "The {0} cannot contain more than 4 digits before the decimal point: " + countVal;
                    return false;
                }
                int countDec = BitConverter.GetBytes(decimal.GetBits(dec)[3])[2];
                if (countDec > 2)
                {
                    ErrorMessage = "The {0} contains too many precision numbers: " + countDec;
                    return false;
                }
            }
            return true;
        }
    }

}
