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
    public class RequestTravellerMap : EntityTypeConfiguration<RequestTraveller>
    {
        public RequestTravellerMap()
        {
            this.HasKey(r => r.Id);
            this.Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(r => r.FamilyName).IsRequired().HasMaxLength(256);
            this.Property(r => r.FirstName).IsRequired().HasMaxLength(256);
            this.Property(r => r.FullName).IsRequired().HasMaxLength(256);
            this.Property(r => r.CompanyID).IsOptional();
            this.Property(r => r.TravelRequestID).IsRequired();
            //this.HasOptional(r => r.PhoneNumber).WithMany().HasForeignKey(r => r.PhoneNumberID);
            //this.HasRequired(r => r.Company).WithMany().HasForeignKey(r => r.CompanyID);
            //this.HasOptional(r => r.FlyerMemberCard).WithMany().HasForeignKey(r => r.FlyerMemberCardID);
        }
    }
}
