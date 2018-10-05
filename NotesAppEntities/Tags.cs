using System;
using System.Collections.Generic;

namespace NotesApp.Api.NotesAppEntities
{
    public partial class Tags
    {
        public Tags()
        {
            TagAlwaysIncludeRelationshipAlwaysIncludeTag = new HashSet<TagAlwaysIncludeRelationship>();
            TagAlwaysIncludeRelationshipChildTag = new HashSet<TagAlwaysIncludeRelationship>();
            TagNoteRelationship = new HashSet<TagNoteRelationship>();
        }

        public int TagId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CreatedByUserId { get; set; }
        public bool IsDeleted { get; set; }

        public Users CreatedByUser { get; set; }
        public ICollection<TagAlwaysIncludeRelationship> TagAlwaysIncludeRelationshipAlwaysIncludeTag { get; set; }
        public ICollection<TagAlwaysIncludeRelationship> TagAlwaysIncludeRelationshipChildTag { get; set; }
        public ICollection<TagNoteRelationship> TagNoteRelationship { get; set; }
    }
}
