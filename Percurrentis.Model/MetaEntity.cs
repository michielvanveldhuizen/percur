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
        public string CreatedBy { get; set; }
        [DateTime]
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DateTime]
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        //logic for creating and maintaining the automatic maintaining of the Entity code
        public MetaEntity()
        {
            CreatedDate = DateTime.Now;
            CreatedBy = "";
            IsDeleted = false;
        }

        public void OnBeforeInsert(string guid)
        {
            this.CreatedDate = DateTime.Now;
            this.CreatedBy = guid;
        }

        public void OnBeforeUpdate(string guid)
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = guid;
        }
    }
}
