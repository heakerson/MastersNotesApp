using System.Linq;
using AutoMapper;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Microsoft.EntityFrameworkCore;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.UserService.Models;

namespace Nerdable.NotesApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly NotesAppContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDbHelper _dbService;

        public UserService(NotesAppContext dbContext, IMapper mapper, IDbHelper dbService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dbService = dbService;
        }

        public IQueryable<Users> GetUserQuery(int userId)
        {
            return _dbContext.Users
                .Include(u => u.Tags)
                .Include(u => u.Notes)
                .Where(u => u.UserId == userId);
        }
        public Response<Users> UpdateSoftDelete(Users entity)
        {
            entity.IsDeleted = true;
            return Response<Users>.BuildResponse(entity);
        }
    }
}
