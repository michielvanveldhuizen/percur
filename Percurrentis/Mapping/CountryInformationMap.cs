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
    public class CountryInformationMap : EntityTypeConfiguration<CountryInformation>
    {
        public CountryInformationMap()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
