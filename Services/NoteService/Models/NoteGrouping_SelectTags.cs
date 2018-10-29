using Nerdable.NotesApi.NotesAppEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteGrouping_SelectTags
    {
        public int NoteId { get; set; }
        public Notes Note { get; set; }
        public IEnumerable<int> TagIds { get; set; }
    }
}
