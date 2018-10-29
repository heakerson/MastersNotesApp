using Nerdable.NotesApi.Services.DirectoryService.Models;
using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.UserService.Models;
using System;
using System.Collections.Generic;

namespace Nerdable.NotesApi.Services.NoteService.Models
{
    public class NoteDetail : NoteBase
    {
        public int NoteId { get; set; }
        public int CreatedByUserId { get; set; }
        public ICollection<TagSummary> Tags { get; set; }
        public ICollection<DirectorySummary> Directories { get; set; }
    }
}
