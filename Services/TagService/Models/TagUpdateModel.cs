using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.TagService.Models
{
    public class TagUpdateModel : TagCreationModel
    {
        public int TagId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
