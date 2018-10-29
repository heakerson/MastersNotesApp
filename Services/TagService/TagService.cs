using System;
using System.Linq;
using AutoMapper;
using Nerdable.DbHelper.Models.Response;
using Microsoft.EntityFrameworkCore;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.UserService;
using Nerdable.NotesApi.Services.RelationshipService;
using Nerdable.DbHelper.Services;

namespace Nerdable.NotesApi.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly NotesAppContext _database;
        private readonly IUserService _userService;
        private readonly IRelationshipService _relationshipService;
        private readonly IDbHelper _dbHelper;

        public TagService(NotesAppContext database, IUserService userService, IRelationshipService relationshipService, IDbHelper dbHelper)
        {
            _database = database;
            _userService = userService;
            _relationshipService = relationshipService;
            _dbHelper = dbHelper;
        }

        public IQueryable<Tags> GetAllTagsQuery()
        {
            return _database.Tags
                .Include(t => t.CreatedByUser)
                .Where(t => t.TagId != GetHomelessTagId()
                    && string.IsNullOrEmpty(t.PathWithIds));
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
