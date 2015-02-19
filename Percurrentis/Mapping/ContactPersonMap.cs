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
    public class ContactPersonMap : EntityTypeConfiguration<ContactPerson>
    {
        public ContactPersonMap()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.Name).IsRequired().HasMaxLength(256);
            this.Property(c => c.EmailAddress).IsOptional().HasMaxLength(256);
            this.Property(c => c.PhoneNumberID).IsRequired();
            this.HasRequired(c => c.PhoneNumber).WithMany().HasForeignKey(c => c.PhoneNumberID);
        }
    }
}
