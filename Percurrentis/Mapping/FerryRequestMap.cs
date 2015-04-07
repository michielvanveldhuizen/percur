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
    public class FerryRequestMap : EntityTypeConfiguration<FerryRequest> 
    {
        public FerryRequestMap()
        {
            this.Map(f =>
            {
                f.MapInheritedProperties();
                f.ToTable("FerryRequest");
            });
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(f => f.DepartureDate).IsRequired();
            this.Property(f => f.DepartureAddressID).IsRequired();
            this.Property(f => f.DestinationAddressID).IsRequired();
            this.Property(f => f.TravelRequestID).IsOptional();
            this.Property(f => f.TravelProposalID).IsOptional();
            this.Property(f => f.LicensePlate).IsOptional().HasMaxLength(256);
            this.Property(f => f.CarHeight).IsOptional();
            this.Property(f => f.CarLength).IsOptional();
            this.Property(f => f.ParentID).IsOptional();
            this.HasRequired(f => f.DepartureAddress).WithMany().HasForeignKey(t => t.DepartureAddressID);
            this.HasRequired(f => f.DestinationAddress).WithMany().HasForeignKey(t => t.DestinationAddressID);
        }
    }
}
