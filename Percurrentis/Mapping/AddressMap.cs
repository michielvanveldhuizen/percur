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
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.AddressName).IsOptional().HasMaxLength(256);
            this.Property(a => a.Street).IsOptional().HasMaxLength(256);
            this.Property(a => a.City).IsOptional().HasMaxLength(256);
            this.Property(a => a.PostalCode).IsOptional().HasMaxLength(256);
            this.Property(a => a.StateProvince).IsOptional().HasMaxLength(256);
            this.Property(a => a.Longitude).IsOptional().HasMaxLength(256);
            this.Property(a => a.Latitude).IsOptional().HasMaxLength(256);

            this.HasOptional(c => c.CountryRegion).WithMany().HasForeignKey(c => c.CountryRegionID);
            //this.HasRequired(a => a.AddressType).WithMany().HasForeignKey(a => a.AddressTypeID);
        }
    }
}
