using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteCreationModel : NoteBase
    {
        public int CreatedByUserId { get; set; }
        public List<TagSummary> Tags { get; set; }
    }
}
