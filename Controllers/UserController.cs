using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.DatabaseServices;
using NotesApp.Api.Services.Response;
using NotesApp.Api.Services.UserService;
using NotesApp.Api.Services.UserService.Models;

namespace NotesApp.Api.Controllers
{
    [ApiController]
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;
        private readonly IDatabaseService _databaseService;
        private readonly NotesAppContext _context;

        public UserController(IUserService userService, IDatabaseService databseService, NotesAppContext context)
        {
            _userService = userService;
            _databaseService = databseService;
            _context = context;
        }


        [HttpPost("[Controller]/Update")]
        public IActionResult UpdateUser([FromBody]UserDetail updateModel)
        {
            var entityResponse = _databaseService.GetEntity<Users>(updateModel.UserId);

            if (entityResponse.Success)
            {
                var updateResponse = _databaseService.UpdateObject<UserDetail,Users>(updateModel, updateModel.UserId);

                return ApiResult(updateResponse);
            }
            else
            {
                return ApiResult(entityResponse);
            }
        }


        [HttpPost("[Controller]/Create")]
        public IActionResult CreateUser([FromBody]UserBaseModel detail)
        {
            var response = _databaseService.AddObject<UserBaseModel, Users>(detail);

            return ApiResult(response);
        }


        [HttpGet("[Controller]/{userId}")]
        public IActionResult GetUser(int userId)
        {
            //var response = _userService.GetUser(userId);
            var response = _databaseService.GetObject<Users, UserDetail>(userId);
            return ApiResult(response);
        }


        [HttpDelete("[Controller]/HardDelete/{userId}")]
        public IActionResult HardDelete(int userId)
        {
            var response = _databaseService.RemoveEntity<Users>(userId);

            return ApiResult(response);
        }
    }
}