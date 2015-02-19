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
    public class NoteCollectionMap : EntityTypeConfiguration<NoteCollection>
    {
        public NoteCollectionMap()
        {
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(n => n.NoteID).IsRequired();
            this.HasMany<Note>(n => n.Notes).WithRequired(n => n.NoteCollection).HasForeignKey(n => n.NoteCollectionID);
        }
    }
}
