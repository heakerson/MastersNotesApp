using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.DatabaseServices;
using NotesApp.Api.Services.Response;
using NotesApp.Api.Services.UserService.Models;

namespace NotesApp.Api.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly NotesAppContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDatabaseService _dbService;

        public UserService(NotesAppContext dbContext, IMapper mapper, IDatabaseService dbService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dbService = dbService;
        }

        public Response<UserBaseModel> CreateUser(UserBaseModel detail)
        {
             //if (detail.IsDeleted)
            //{
            //    return Response<UserDetail>.BuildResponse(detail, false, ReturnCode.InvalidInput, "Cannot Create a deleted new user");
            //}

            //if (detail.UserId == 0)
            //{


            try
            {
                var entity = _mapper.Map<Users>(detail);

                _dbContext.Add(entity);
                _dbContext.SaveChanges();

                var returnData = _mapper.Map<UserDetail>(_dbContext.Users.Find(entity.UserId));

                return Response<UserBaseModel>.BuildResponse(returnData);
            }
            catch
            {
                return Response<UserBaseModel>.BuildResponse(detail, false, ReturnCode.Fail, "Failed creating the new user");
            }


            //}
            //else
            //{
            //    return Response<UserBaseModel>.BuildResponse(detail, false, ReturnCode.InvalidInput, "Cannot Create a new user with a specific Id. Leave user id as 0.");
            //}
        }

        public Response<UserBaseModel> UpdateUser(UserUpdateModel detail)
        {

            try
            {
                var getUserEntityResponse = GetUserEntity(detail.UserId);

                if (getUserEntityResponse.Success)
                {
                    getUserEntityResponse.Data = _mapper.Map<Users>(detail);
                    _dbContext.Update(getUserEntityResponse.Data);
                    _dbContext.SaveChanges();


                    var returnData = _mapper.Map<UserDetail>(_dbContext.Users.Find(getUserEntityResponse.Data.UserId));

                    return Response<UserBaseModel>.BuildResponse(returnData);
                }
                else
                {
                    return Response<UserBaseModel>.BuildResponse(new UserDetail() { UserId = detail.UserId }, false, getUserEntityResponse.ReturnCode, getUserEntityResponse.ReturnMessage);
                }
            }
            catch
            {
                return Response<UserBaseModel>.BuildResponse(detail, false, ReturnCode.Fail, "Failed updating the user");
            }

        }

        public Response<UserDetail> GetUser(int userId)
        {
            var userEntityResponse = GetUserEntity(userId);

            if (userEntityResponse.Success)
            {
                UserDetail detail = _mapper.Map<UserDetail>(userEntityResponse.Data);
                return Response<UserDetail>.BuildResponse(detail);
            }
            else
            {
                return Response<UserDetail>.BuildResponse(new UserDetail() { UserId = userId }, false, userEntityResponse.ReturnCode, userEntityResponse.ReturnMessage);
            }

        }

        public Response<Users> GetUserEntity(int userId)
        {
            if (UserExists(userId))
            {
                try
                {
                    var userEntity = _dbContext.Users
                        .Include(u => u.Tags)
                        .Include(u => u.Notes)
                        .Where(u => u.UserId == userId)
                        .FirstOrDefault();

                    return Response<Users>.BuildResponse(userEntity);
                }
                catch
                {
                    return Response<Users>.BuildResponse(new Users() { UserId = userId }, false, ReturnCode.Fail, $"Could not retrieve user entity with Id {userId} from the database");
                }
            }
            else
            {
                return Response<Users>.BuildResponse(new Users() { UserId = userId}, false, ReturnCode.DoesNotExist, $"The user with Id {userId} does not exist in the database");
            }
        }


        public bool UserExists(int userId)
        {
            return _dbContext.Users.Any(u => u.UserId == userId);
        }

        public bool UserExistsAndNotDeleted(int userId)
        {
            return _dbContext.Users.Any(u => u.UserId == userId && !u.IsDeleted);
        }

        public bool UserExistsButSoftDeleted(int userId)
        {
            return _dbContext.Users.Any(u => u.UserId == userId && u.IsDeleted);
        }
    }
}
