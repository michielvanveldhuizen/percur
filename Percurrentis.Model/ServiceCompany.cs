// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;

namespace Percurrentis.Model
{
    public class ServiceCompany : CompanyBase
    {
        //future foreign key
        public int? ContractCollection { get; set; }
        public int ServiceCompanyTypeID { get; set; }
        [ValidateObject]
        public virtual ServiceCompanyType ServiceCompanyType { get; set; }
    }
}
