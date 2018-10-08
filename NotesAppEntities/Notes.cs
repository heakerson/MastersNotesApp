using System;
using System.Collections.Generic;

namespace Nerdable.NotesApi.NotesAppEntities
{
    public partial class Notes
    {
        public Notes()
        {
            TagNoteRelationship = new HashSet<TagNoteRelationship>();
        }

        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Public { get; set; }
        public int? CreatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastUpdated { get; set; }

        public Users CreatedByUser { get; set; }
        public ICollection<TagNoteRelationship> TagNoteRelationship { get; set; }
    }
}
