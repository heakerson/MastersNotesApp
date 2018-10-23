using System;
using System.Linq;
using AutoMapper;
using Nerdable.DbHelper.Models.Response;
using Microsoft.EntityFrameworkCore;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.UserService;

namespace Nerdable.NotesApi.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly NotesAppContext _database;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public TagService(NotesAppContext database, IMapper mapper, IUserService userService)
        {
            _database = database;
            _mapper = mapper;
            _userService = userService;
        }

        public IQueryable<Tags> GetAllTagsQuery()
        {
            return _database.Tags
                .Include(t => t.CreatedByUser);
        }

        public IQueryable<Tags> GetTagQuery(int tagId)
        {
            return GetAllTagsQuery()
                .Where(t => t.TagId == tagId);
        }

        public IQueryable<Tags> GetTagsBySearch_Query(string searchTerm)
        {
            return GetAllTagsQuery()
                .Where(t => t.Title.ToLower().Contains(searchTerm.ToLower()) && t.TagId != GetHomelessTagId());
        }

        public int GetHomelessTagId()
        {
            return _database.Tags.Where(t => t.Title == "HOMELESS NOTES").FirstOrDefault().TagId;
        }

        public Response<Tags> UpdateSoftDelete(Tags entity)
        {
            entity.IsDeleted = true;
            return Response<Tags>.BuildResponse(entity);
        }
    }
}
