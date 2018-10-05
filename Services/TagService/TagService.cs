using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.Response;
using NotesApp.Api.Services.TagService.Models;
using NotesApp.Api.Services.UserService;

namespace NotesApp.Api.Services.TagService
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

        public Response<TagCreationModel> CreateTag(TagCreationModel detail)
        {
            try
            {
                if (_userService.UserExistsButSoftDeleted(detail.CreatedByUserId))
                {
                    return Response<TagCreationModel>.BuildResponse(detail, false, ReturnCode.InvalidInput, $"User with Id {detail.CreatedByUserId} has been deleted");
                }
                else if (_userService.UserExistsAndNotDeleted(detail.CreatedByUserId))
                {
                    var entity = _mapper.Map<Tags>(detail);
                    var addResult = _database.Add(entity);
                    var savedResult = _database.SaveChanges();

                    var createdUser = _database.Tags
                        .Include(t => t.CreatedByUser)
                        .Where(t => t.TagId == entity.TagId)
                        .FirstOrDefault();

                    var returnData = _mapper.Map<TagDetail>(_database.Tags.Find(createdUser.TagId));

                    return Response<TagCreationModel>.BuildResponse(returnData);
                }
                else
                {
                    return Response<TagCreationModel>.BuildResponse(detail, false, ReturnCode.InvalidInput, $"User with Id {detail.CreatedByUserId} does not exist.");
                }
            }
            catch
            {
                return Response<TagCreationModel>.BuildResponse(detail, false, ReturnCode.Fail, "Failed creating the new tag");
            }


        }

        public Response<TagDetail> UpdateTag(TagDetail detail)
        {
            if (detail.TagId > 0)
            {
                try
                {
                    var userEntity = _mapper.Map<Tags>(detail);

                    _database.Update(userEntity);
                    _database.SaveChanges();

                    var returnData = _mapper.Map<TagDetail>(_database.Users.Find(userEntity.TagId));

                    return Response<TagDetail>.BuildResponse(returnData);
                }
                catch
                {
                    return Response<TagDetail>.BuildResponse(detail, false, ReturnCode.Fail, "Failed updating the tag");
                }
            }
            else
            {
                return Response<TagDetail>.BuildResponse(detail, false, ReturnCode.InvalidInput, "Must include a valid tag id to update");
            }
        }

        public TagDetail GetTag(int tagId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tags> GetTagQuery(int tagId)
        {
            return _database.Tags
                .Include(t => t.CreatedByUser)
                .Where(t => t.TagId == tagId);
        }

        public Response<Tags> SoftDeleteUpdate(Tags entity)
        {
            entity.IsDeleted = true;
            return Response<Tags>.BuildResponse(entity);
        }
    }
}
