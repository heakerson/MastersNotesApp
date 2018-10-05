using System;
using System.Collections.Generic;

namespace NotesApp.Api.NotesAppEntities
{
    public partial class Users
    {
        public Users()
        {
            Notes = new HashSet<Notes>();
            Tags = new HashSet<Tags>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }

        public ICollection<Notes> Notes { get; set; }
        public ICollection<Tags> Tags { get; set; }
    }
}
