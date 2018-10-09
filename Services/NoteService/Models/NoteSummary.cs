using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteSummary
    {
        public string Title { get; set; }
        public int NoteId { get; set; }
        public bool Public { get; set; }
        public bool IsDeleted { get; set; }
    }
}
