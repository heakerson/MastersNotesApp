using Nerdable.DbHelper.Services;
using Microsoft.AspNetCore.Mvc;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.UserService;
using Nerdable.NotesApi.Services.UserService.Models;

namespace Nerdable.NotesApi.Controllers
{
    [ApiController]
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;
        private readonly IDbHelper _dbHelper;
        private readonly NotesAppContext _context;

        public UserController(IUserService userService, IDbHelper dbHelper, NotesAppContext context)
        {
            _userService = userService;
            _dbHelper = dbHelper;
            _context = context;
        }


        [HttpPost("[Controller]/Create")]
        public IActionResult CreateUser([FromBody]UserBaseModel detail)
        {
            var response = _dbHelper.AddObject<UserBaseModel, Users>(detail);

            return ApiResult(response);
        }

        [HttpPost("[Controller]/Update")]
        public IActionResult UpdateUser([FromBody]UserDetail updateModel)
        {
            var updateResponse = _dbHelper.UpdateObject<UserDetail,Users>(updateModel, updateModel.UserId);

            return ApiResult(updateResponse);
        }


        [HttpGet("[Controller]/{userId}")]
        public IActionResult GetUser(int userId)
        {
            var query = _userService.GetUserQuery(userId);
            var userResponse = _dbHelper.GetObject<Users,UserDetail>(userId);

            return ApiResult(userResponse);
        }


        [HttpDelete("[Controller]/HardDelete/{userId}")]
        public IActionResult HardDeleteUser(int userId)
        {
            var response = _dbHelper.RemoveEntity<Users>(userId);

            return ApiResult(response);
        }

        [HttpPost("[Controller]/SoftDelete/{userId}")]
        public IActionResult SoftDeleteUser(int userId)
        {
            var response = _dbHelper.UpdateObject<Users>(userId, _userService.UpdateSoftDelete);

            return ApiResult(response);
        }
    }
}