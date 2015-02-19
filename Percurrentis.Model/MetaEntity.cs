// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System;

namespace Percurrentis.Model
{
    public abstract class MetaEntity
    {
        [DateTime]
        public DateTime CreatedDate { get; set; }
        [Mandatory]
        public int CreatedBy { get; set; }
        [DateTime]
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        [DateTime]
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        //logic for creating and maintaining the automatic maintaining of the Entity code
        public MetaEntity()
        {
            CreatedDate = DateTime.Now;
            CreatedBy = 1;
            IsDeleted = false;
        }

        public void OnBeforeInsert()
        {
            this.CreatedDate = DateTime.Now;
            this.CreatedBy = 1;
        }

        public void OnBeforeUpdate()
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = 1;
        }
    }
}
