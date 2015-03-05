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
    public class RentalCarRequestMap : EntityTypeConfiguration<RentalCarRequest>
    {
        public RentalCarRequestMap()
        {
            this.HasKey(r => r.Id);
            this.Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(r => r.StartDate).IsRequired();
            this.Property(r => r.EndDate).IsRequired();
            this.Property(r => r.AddressID).IsRequired();
            this.Property(r => r.DriverID).IsRequired();
            this.Property(r => r.TravelRequestID).IsRequired();
            this.HasRequired(r => r.Driver).WithMany().HasForeignKey(r => r.DriverID);
            this.HasOptional(r => r.SecondaryDriver).WithMany().HasForeignKey(r => r.SecondaryDriverID);
            this.HasRequired(r => r.Address).WithMany().HasForeignKey(r => r.AddressID);
            this.Property(a => a.Cost).IsOptional();
        }
    }
}
