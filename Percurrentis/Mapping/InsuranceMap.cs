// <copyright company=CSi Romania SRL>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Peter Feddema</author>
// <summary>Mapping classes for the migration to the database</summary>

using Percurrentis.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Percurrentis.Mapping
{
    public class InsuranceMap : EntityTypeConfiguration<Insurance>
    {
        public InsuranceMap()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.InsuranceNumber).IsRequired().HasMaxLength(256);
            this.Property(p => p.InsureeGUID).IsRequired().HasMaxLength(256);
            this.Property(p => p.ExpirationDate).IsRequired();
        }
    }
}