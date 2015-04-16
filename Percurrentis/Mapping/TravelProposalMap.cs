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
    public class TravelProposalMap : EntityTypeConfiguration<TravelProposal>
    {
        public TravelProposalMap()
        {
            /*this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IsApproved).IsRequired();*/

            this.HasMany<Accommodation>(t => t.Accommodations)
            .WithOptional(a => a.TravelProposal).HasForeignKey(r => r.TravelProposalID);
            this.HasMany<FlightRequest>(t => t.FlightRequests)
            .WithOptional(f => f.TravelProposal).HasForeignKey(f => f.TravelProposalID);
            this.HasMany<FerryRequest>(t => t.FerryRequests)
            .WithOptional(f => f.TravelProposal).HasForeignKey(f => f.TravelProposalID);
            this.HasMany<RentalCarRequest>(t => t.RentalCarRequests)
            .WithOptional(r => r.TravelProposal).HasForeignKey(r => r.TravelProposalID);
            this.HasMany<TaxiRequest>(t => t.TaxiRequests)
            .WithOptional(t => t.TravelProposal).HasForeignKey(t => t.TravelProposalID);
            this.HasMany<EuroTunnelRequest>(t => t.EuroTunnelRequests)
            .WithOptional(e => e.TravelProposal).HasForeignKey(e => e.TravelProposalID);

            this.HasOptional(t => t.TravelRequestApproval).WithMany().HasForeignKey(t => t.TravelRequestApprovalID);
        }
    }
}