// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Mapping classes for the migration to the database</summary>

using Percurrentis.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Percurrentis.Mapping
{
    public class ServiceCompanyMap : EntityTypeConfiguration<ServiceCompany>
    {
        public ServiceCompanyMap()
        {
            this.Map(s =>
            {
                s.MapInheritedProperties();
                s.ToTable("ServiceCompany");
            });
            this.HasKey(s => s.Id);
            this.Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.Name).IsRequired().HasMaxLength(256);
            this.HasOptional(s => s.Address).WithMany().HasForeignKey(s => s.AddressID);
            this.HasRequired(s => s.ServiceCompanyType).WithMany().HasForeignKey(s => s.ServiceCompanyTypeID);
        }
    }
}
