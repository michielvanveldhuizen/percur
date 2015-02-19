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
    public class PhoneNumberMap : EntityTypeConfiguration<PhoneNumber>
    {
        public PhoneNumberMap()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Number).IsRequired();
            this.HasOptional(p => p.CountryInformation).WithMany().HasForeignKey(p => p.CountryInformationID);  
        }
    }
}
