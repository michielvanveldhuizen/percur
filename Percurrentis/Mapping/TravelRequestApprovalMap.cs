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
    public class TravelRequestApprovalMap : EntityTypeConfiguration<TravelRequestApproval>
    {
        public TravelRequestApprovalMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.ApprovedBy).IsOptional().HasMaxLength(256);
            this.Property(t => t.ApprovalDate).IsOptional();
            this.Property(t => t.ApprovedBy).IsOptional().HasMaxLength(256);
            this.Property(t => t.Archived).IsRequired();
            this.Property(t => t.Flag).IsRequired();
            this.Property(t => t.NotificationSent).IsRequired();
            this.Property(t => t.HasApproved).IsRequired();
            this.Property(t => t.COOApproved).IsRequired();
            
        }
    }
}
