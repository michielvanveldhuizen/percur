// <copyright company=CSi Romania SRL>
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Michiel van Veldhuizen</author>
// <summary>Mapping classes for the migration to the database</summary>

using Percurrentis.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Percurrentis.Mapping
{
    public class MailErrorMap : EntityTypeConfiguration<MailError>
    {
        public MailErrorMap()
        {
            this.HasKey(p => p.Id);
        }
    }
}