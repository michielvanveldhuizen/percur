// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Validation classes for the database</summary>

using System;
using System.ComponentModel.DataAnnotations;

namespace Percurrentis.Model.Validation
{
    //[AttributeUsage(AttributeTargets.Class)]
    public class AddressAttribute : ValidationAttribute
    {
        private const string DefaultErrormessage = "The data in the Address entity is invalid";
        public AddressAttribute()
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
                return true;
            }
            var addr = value as Address;
            if ((addr.Street != null) || (addr.City != null) || (addr.Latitude != null) || (addr.Longitude != null) || (addr.PostalCode != null) || (addr.StateProvince != null))
            {
                if ((addr.AddressName == null) || (addr.Street == null) || (addr.City == null))// || (addr.AddressType == null))
                {
                    ErrorMessage = "Not all the required fields have been correctly filled in";
                    return false;
                }
            }
            else if ((addr.AddressName == null) && (addr.City == null) && (addr.Latitude == null) && (addr.Longitude == null) && (addr.PostalCode == null) && (addr.StateProvince == null) && (addr.Street == null))
            {
                ErrorMessage = "The other shit is going bad";
                return false;
            }
            return true;
        }
    }
}
