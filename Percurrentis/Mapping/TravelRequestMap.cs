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
    public class TravelRequestMap : EntityTypeConfiguration<TravelRequest>
    {
        public TravelRequestMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.QuoteOrOrderNumber).IsOptional().HasMaxLength(256);
            this.Property(t => t.Purpose).IsRequired().HasMaxLength(1024);
            this.Property(t => t.IsArchived).IsRequired();
            this.Property(t => t.IsApproved).IsRequired();
            this.Property(t => t.CustomerOrProspectID).IsRequired();
            this.Property(t => t.ApplicantID).IsRequired();
            this.HasMany<Accommodation>(t => t.Accommodations)
            .WithRequired(a => a.TravelRequest).HasForeignKey(r => r.TravelRequestID);
            this.HasMany<RentalCarRequest>(t => t.RentalCarRequests)
            .WithRequired(r => r.TravelRequest).HasForeignKey(r => r.TravelRequestID);
            this.HasMany<FerryRequest>(t => t.FerryRequests)
            .WithRequired(f => f.TravelRequest).HasForeignKey(f => f.TravelRequestID);
            this.HasMany<TaxiRequest>(t => t.TaxiRequests)
            .WithRequired(t => t.TravelRequest).HasForeignKey(t => t.TravelRequestID);
            this.HasMany<EuroTunnelRequest>(t => t.EuroTunnelRequests)
            .WithRequired(e => e.TravelRequest).HasForeignKey(e => e.TravelRequestID);
            this.HasMany<FlightRequest>(t => t.FlightRequests)
            .WithRequired(f => f.TravelRequest).HasForeignKey(f => f.TravelRequestID);
            this.HasMany<RequestTraveller>(t => t.RequestTravellers)
            .WithRequired(r => r.TravelRequest).HasForeignKey(r => r.TravelRequestID);
            this.HasRequired(t => t.CustomerOrProspect).WithMany().HasForeignKey(t => t.CustomerOrProspectID);
            this.HasOptional(t => t.TravelRequestApproval).WithMany().HasForeignKey(t => t.TravelRequestApprovalID);

            this.HasOptional(c => c.Country).WithMany().HasForeignKey(c => c.CountryID);
            //this.HasRequired(t => t.Superior).WithMany().HasForeignKey(t => t.SuperiorID);
            //this.HasRequired(t => t.Applicant).WithMany().HasForeignKey(t => t.ApplicantID);
            //this.HasOptional(t => t.NoteCollection).WithMany().HasForeignKey(t => t.NoteCollectionID);
        }
    }
}
