using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.Response;
using NotesApp.Api.Services.UserService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.UserService
{
    public interface IUserService
    {
        Response<UserBaseModel> CreateUser(UserBaseModel user);
        Response<UserBaseModel> UpdateUser(UserUpdateModel user);
        Response<UserDetail> GetUser(int userId);
        Response<Users> GetUserEntity(int userId);

        bool UserExists(int userId);
        bool UserExistsAndNotDeleted(int userId);
        bool UserExistsButSoftDeleted(int userId);
    }
}
