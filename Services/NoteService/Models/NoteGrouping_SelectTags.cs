using Nerdable.NotesApi.NotesAppEntities;
using System.Collections.Generic;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteGrouping_SelectTags
    {
        public int NoteId { get; set; }
        public Notes Note { get; set; }
        public IEnumerable<int> TagIds { get; set; }
    }
}
