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
    public class AccommodationMap : EntityTypeConfiguration<Accommodation>
    {
        public AccommodationMap()
        { 
            this.HasKey(a => a.Id);
            this.Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.Cost).IsOptional();
            this.HasOptional(a => a.Address).WithMany().HasForeignKey(a => a.AddressID);
        }
    }
}
