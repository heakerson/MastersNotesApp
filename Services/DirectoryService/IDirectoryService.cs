using Nerdable.DbHelper.Models.Response;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.DirectoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.DirectoryService
{
    public interface IDirectoryService
    {
        IQueryable<Tags> GetDirectoryQuery(int directoryId);
        IQueryable<Tags> GetAllDirectoriesQuery();
        IQueryable<Tags> GetRootDirectoryQuery(int directoryId);
        IQueryable<Tags> GetAllChildDirectoriesQuery(int parentDirectory);
        IQueryable<Tags> GetDirectChildDirectoriesQuery(int parentId);

        int GetHomelessDirectoryId();
        int GetMainDirectoryRootId();

        Response<Tags> AddNewDirectoryEntity(DirectoryCreationModel model);
        Response<Tags> MoveDirectory(int childId, int newParentId);
        Response<Tags> UpdateDirectoryParent(Tags entity, int parentId);
        Response<Tags> UpdateDirectoryPath(Tags entity, string newPathWithIds, string newPathWithTitles);
    }
}
