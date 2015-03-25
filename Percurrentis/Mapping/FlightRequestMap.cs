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
    public class FlightRequestMap : EntityTypeConfiguration<FlightRequest>
    {
        public FlightRequestMap()
        {
            this.HasKey(f => f.Id);
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(f => f.DepartureDate).IsRequired();
            this.Property(f => f.HasLargeCabinLuggage).IsRequired();
            this.Property(f => f.HasSpecialEquipment).IsRequired();
            this.Property(f => f.DepartureAddressID).IsRequired();
            this.Property(f => f.DestinationAddressID).IsRequired();
            this.Property(f => f.TravelRequestID).IsOptional();
            this.Property(f => f.TravelProposalID).IsOptional();
            this.Property(f => f.ParentID).IsOptional();
            this.HasOptional(f => f.FlyerMemberCard).WithMany().HasForeignKey(f => f.FlyerMemberCardID);
            this.HasRequired(f => f.DepartureAddress).WithMany().HasForeignKey(f => f.DepartureAddressID);
            this.HasRequired(f => f.DestinationAddress).WithMany().HasForeignKey(f => f.DestinationAddressID);
        }
    }
}
