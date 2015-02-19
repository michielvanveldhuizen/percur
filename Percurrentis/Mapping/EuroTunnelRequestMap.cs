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
    public class EuroTunnelRequestMap : EntityTypeConfiguration<EuroTunnelRequest>
    {
        public EuroTunnelRequestMap()
        {
            this.Map(e =>
            {
                e.MapInheritedProperties();
                e.ToTable("EuroTunnelRequest");
            });
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(e => e.DepartureDate).IsRequired();
            this.Property(e => e.DepartureAddressID).IsRequired();
            this.Property(e => e.DestinationAddressID).IsRequired();
            this.Property(e => e.TravelRequestID).IsRequired();
            this.Property(e => e.LicensePlate).IsOptional().HasMaxLength(256);
            this.HasRequired(e => e.DepartureAddress).WithMany().HasForeignKey(t => t.DepartureAddressID);
            this.HasRequired(e => e.DestinationAddress).WithMany().HasForeignKey(t => t.DestinationAddressID);

        }
    }
}
