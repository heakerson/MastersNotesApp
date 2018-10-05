using System;
using System.Collections.Generic;

namespace NotesApp.Api.NotesAppEntities
{
    public partial class TagNoteRelationship
    {
        public int TagId { get; set; }
        public int NoteId { get; set; }

        public Notes Note { get; set; }
        public Tags Tag { get; set; }
    }
}
