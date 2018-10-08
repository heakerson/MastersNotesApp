using Nerdable.DbHelper.Models.Response;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.UserService.Models;
using System.Linq;

namespace Nerdable.NotesApi.Services.UserService
{
    public interface IUserService
    {
        IQueryable<Users> GetUserQuery(int userId);

        Response<Users> UpdateSoftDelete(Users entity);
    }
}
