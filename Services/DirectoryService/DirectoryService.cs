using Microsoft.EntityFrameworkCore;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.DirectoryService.Models;
using Nerdable.NotesApi.Services.TagService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.DirectoryService
{
    public class DirectoryService : IDirectoryService
    {
        private readonly ITagService _tagService;
        private readonly IDbHelper _dbHelper;
        private readonly NotesAppContext _context;

        public DirectoryService(NotesAppContext database, ITagService tagService, IDbHelper dbHelper)
        {
            _tagService = tagService;
            _dbHelper = dbHelper;
            _context = database;
        }

        public IQueryable<Tags> GetDirectoryQuery(int directoryId)
        {
            return GetAllDirectoriesQuery()
                    .Where(d => d.TagId == directoryId);
        }

        public IQueryable<Tags> GetAllDirectoriesQuery()
        {
            return _context.Tags
                    .Include(t => t.CreatedByUser)
                    .Where(t => !string.IsNullOrEmpty(t.PathWithTitles));
        }

        public IQueryable<Tags> GetRootDirectoryQuery(int directoryId)
        {
            return GetDirectoryQuery(directoryId)
                    .Where(t => string.IsNullOrEmpty(t.ParentTitle));
        }

        public IQueryable<Tags> GetAllChildDirectoriesQuery(int parentId)
        {
            var response = _dbHelper.GetEntityByQuery(GetDirectoryQuery(parentId));

            if (response.Success)
            {
                var path = response.Data.PathWithIds;

                return GetAllDirectoriesQuery()
                    .Where(d => d.PathWithIds.Contains(path) && d.PathWithIds != path);
            }

            return null;
        }


        public IQueryable<Tags> GetDirectChildDirectoriesQuery(int parentId)
        {
            return GetAllDirectoriesQuery()
                    .Where(d => d.ParentId == parentId);
        }


        public Response<Tags> AddNewDirectoryEntity(DirectoryCreationModel model)
        {
            var createEntityResponse = _dbHelper.AddObject<DirectoryCreationModel, Tags>(model);

            if (createEntityResponse.Success)
            {
                var updateParentsResposne = UpdateDirectoryParent(createEntityResponse.Data, model.ParentId);

                if (!updateParentsResposne.Success)
                {
                    _dbHelper.RemoveEntity<Tags>(createEntityResponse.Data.TagId);
                }

                return updateParentsResposne;
            }
            else
            {
                return createEntityResponse;
            }
        }


        public Response<Tags> MoveDirectory(int childDirectoryId, int newParentId)
        {
            var entityResponse = _dbHelper.GetEntityByQuery(GetDirectoryQuery(childDirectoryId));

            if (entityResponse.Success)
            {
                var childrenResponse = _dbHelper.GetEntitiesByQuery(GetAllChildDirectoriesQuery(childDirectoryId));

                if (childrenResponse.Success)
                {
                    string oldParentPathWithIds = entityResponse.Data.PathWithIds;
                    string oldParentPathWithTitles = entityResponse.Data.PathWithTitles;

                    var updateParent = UpdateDirectoryParent(entityResponse.Data, newParentId);

                    if (updateParent.Success)
                    {
                        var children = childrenResponse.Data;
                        var parent = updateParent.Data;

                        foreach (Tags child in children)
                        {
                            var newPathWithIds = child.PathWithIds.Replace(oldParentPathWithIds, parent.PathWithIds);
                            var newPathWithTitles = child.PathWithTitles.Replace(oldParentPathWithTitles, parent.PathWithTitles);

                            var updateChildResponse = UpdateDirectoryPath(child, newPathWithIds, newPathWithTitles);
                        }

                        return _dbHelper.GetEntityByQuery(GetDirectoryQuery(childDirectoryId));
                    }
                    else
                    {
                        return updateParent;
                    }
                }
                else if(childrenResponse.ReturnCode == ReturnCode.NoEntitiesMatchQuery)
                {
                    return UpdateDirectoryParent(entityResponse.Data, newParentId);
                }
                else
                {
                    return Response<Tags>.BuildResponse(null, false, childrenResponse.ReturnCode, "Failed to get child directores. Message: " + childrenResponse.ReturnMessage);
                }
            }
            else
            {
                return entityResponse;
            }
        }


        public Response<Tags> UpdateDirectoryParent(Tags entity, int parentId)
        {
            var parentDirectoryResponse = _dbHelper.GetEntityByQuery(GetDirectoryQuery(parentId));

            if (parentDirectoryResponse.Success)
            {
                var parent = parentDirectoryResponse.Data;

                entity.PathWithTitles = $"{parent.PathWithTitles}/{entity.Title}";
                entity.PathWithIds = $"{parent.PathWithIds}/{entity.TagId}";
                entity.ParentTitle = parent.Title;
                entity.ParentId = parentId;

                var saveResponse = _dbHelper.SaveObject(entity);

                //var updateResponse = _dbHelper.UpdateObject<Tags, Tags>(entity, entity.TagId);

                return saveResponse;
            }
            else
            {
                return parentDirectoryResponse;
            }
        }

        public Response<Tags> UpdateDirectoryPath(Tags entity, string newPathWithIds, string newPathWithTitles)
        {
            entity.PathWithTitles = newPathWithTitles;
            entity.PathWithIds = newPathWithIds;

            var saveResponse = _dbHelper.SaveObject(entity);

            return saveResponse;
        }

        public int GetHomelessDirectoryId()
        {
            return _tagService.GetHomelessTagId();
        }

        public int GetMainDirectoryRootId()
        {
            return _dbHelper.GetDbSet<Tags>().Data
                .Where(t => t.Title == "Everything")
                .FirstOrDefault()
                .TagId;
        }
    }
}
