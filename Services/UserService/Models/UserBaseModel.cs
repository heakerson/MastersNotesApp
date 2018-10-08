using Nerdable.NotesApi.NotesAppEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.UserService.Models
{
    public class UserBaseModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
