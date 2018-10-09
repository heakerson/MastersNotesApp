using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.TagService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.UserService.Models
{
    public class UserDetail : UserUpdateModel
    {
        public ICollection<TagSummary> TagsCreated { get; set; }
        public ICollection<NoteSummary> NotesCreated { get; set; }
    }
}
