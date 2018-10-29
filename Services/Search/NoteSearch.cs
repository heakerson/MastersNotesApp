using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.Search
{
    public class NoteSearch
    {
        public int DirectoryId { get; set; }
        public List<int> DirectFilterIds { get; set; }
    }
}
