using Nerdable.NotesApi.Services.TagService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.DirectoryService.Models
{
    public class DirectoryDetail : TagDetail
    {
        public string PathWithTitles { get; set; }
        public string PathWithIds { get; set; }
        public int ParentId { get; set; }
        public string ParentTitle { get; set; }

        public List<int> ParentIds { get; set; } = new List<int>();
    }
}
