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
    public class FlyerMemberCardMap : EntityTypeConfiguration<FlyerMemberCard>
    {
        public FlyerMemberCardMap()
        {
            this.HasKey(f => f.Id);
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(f => f.FMCNumber).IsOptional().HasMaxLength(256);
        }
    }
}
