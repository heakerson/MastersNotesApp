using System;
using System.Collections.Generic;

namespace Nerdable.NotesApi.NotesAppEntities
{
    public partial class Tags
    {
        public Tags()
        {
            InverseParent = new HashSet<Tags>();
            TagNoteRelationship = new HashSet<TagNoteRelationship>();
        }

        public int TagId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CreatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public string PathWithTitles { get; set; }
        public string PathWithIds { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }

        public Users CreatedByUser { get; set; }
        public Tags Parent { get; set; }
        public ICollection<Tags> InverseParent { get; set; }
        public ICollection<TagNoteRelationship> TagNoteRelationship { get; set; }
    }
}
