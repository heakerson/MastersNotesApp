using NotesApp.Api.Services.TagService.Models;
using NotesApp.Api.Services.UserService.Models;
using System;
using System.Collections.Generic;

namespace NotesApp.Api.Services.NoteService.Models
{
    public class NoteDetail
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Public { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastUpdated { get; set; }

        public UserDetail CreatedBy { get; set; }
        public ICollection<TagDetail> Tags { get; set; }
    }
}
