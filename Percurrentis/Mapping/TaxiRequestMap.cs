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
    public class TaxiRequestMap : EntityTypeConfiguration<TaxiRequest>
    {
        public TaxiRequestMap()
        {
            this.Map(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("TaxiRequest");
            });
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.DepartureDate).IsRequired();
            this.Property(t => t.DepartureAddressID).IsRequired();
            this.Property(t => t.DestinationAddressID).IsRequired();
            this.Property(t => t.TravelRequestID).IsOptional();
            this.Property(t => t.TravelProposalID).IsOptional();
            this.HasRequired(t => t.DepartureAddress).WithMany().HasForeignKey(t => t.DepartureAddressID);
            this.HasRequired(t => t.DestinationAddress).WithMany().HasForeignKey(t => t.DestinationAddressID);
        }
    }
}
