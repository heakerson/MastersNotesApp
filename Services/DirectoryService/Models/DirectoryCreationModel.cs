using Nerdable.NotesApi.Services.TagService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.DirectoryService.Models
{
    public class DirectoryCreationModel : TagCreationModel
    {
        [Required]
        public int ParentId { get; set; }
    }
}
