using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.TagService.Models
{
    public class TagCreationModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
