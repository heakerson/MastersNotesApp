using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.UserService.Models
{
    public class UserUpdateModel : UserBaseModel
    {
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
