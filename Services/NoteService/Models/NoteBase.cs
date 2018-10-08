using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Public { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
