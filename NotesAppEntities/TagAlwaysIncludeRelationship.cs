using System;
using System.Collections.Generic;

namespace Nerdable.NotesApi.NotesAppEntities
{
    public partial class TagAlwaysIncludeRelationship
    {
        public int AlwaysIncludeTagId { get; set; }
        public int ChildTagId { get; set; }

        public Tags AlwaysIncludeTag { get; set; }
        public Tags ChildTag { get; set; }
    }
}
