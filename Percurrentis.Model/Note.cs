// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;

namespace Percurrentis.Model
{
    public class Note
    {
        public int Id { get; set; }
        [Mandatory]
        [_1024]
        public string Text { get; set; }
        public int NoteCollectionID { get; set; }
        public virtual NoteCollection NoteCollection { get; set; }
    }
}
