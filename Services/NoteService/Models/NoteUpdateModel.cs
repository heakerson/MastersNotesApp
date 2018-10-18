using Nerdable.NotesApi.Services.TagService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteUpdateModel : NoteBase
    {
        public int NoteId { get; set; }
        public int CreatedByUserId { get; set; }
        //public List<TagSummary> Tags { get; set; }
    }
}
