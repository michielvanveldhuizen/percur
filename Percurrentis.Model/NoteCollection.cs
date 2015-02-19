// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation;
using System.Collections.Generic;

namespace Percurrentis.Model
{
    public class NoteCollection
    {
        public int Id { get; set; }
        public int NoteID { get; set; }
        [ValidateObject]
        public virtual Note Note { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
