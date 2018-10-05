﻿using NotesApp.Api.Services.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.TagService.Models
{
    public class TagDetail : TagUpdateModel
    {
        public string CreatedByUserName { get; set; }
        public ICollection<TagSummary> TagsToAlwaysInclude { get; set; }
    }
}