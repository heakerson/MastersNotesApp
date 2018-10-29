using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.DirectoryService;
using Nerdable.NotesApi.Services.DirectoryService.Models;
using Nerdable.NotesApi.Services.TagService;

namespace Nerdable.NotesApi.Controllers
{
    [ApiController]
    public class DirectoryController : ApiBaseController
    {
        private readonly IDirectoryService _directoryService;
        private readonly IDbHelper _dbHelper;
        private readonly ITagService _tagService;

        public DirectoryController(ITagService tagService, IDbHelper dbHelper, IDirectoryService directoryService)
        {
            _directoryService = directoryService;
            _dbHelper = dbHelper;
            _tagService = tagService;
        }

        [HttpGet("[Controller]/{directoryId}")]
        public IActionResult GetDirectory(int directoryId)
        {
            IQueryable<Tags> query = _directoryService.GetDirectoryQuery(directoryId);
            var response = _dbHelper.GetObjectByQuery<Tags, DirectoryDetail>(query);

            return ApiResult(response);
        }

        [HttpPost("[controller]/Create")]
        public IActionResult CreateDirectory([FromBody]DirectoryCreationModel model)
        {
            var createResponse = _directoryService.AddNewDirectoryEntity(model);

            return ApiResult(createResponse);
        }


        [HttpPost("[controller]/{childDirectoryId}/Move/{newParentId}")]
        public IActionResult MoveDirectory(int childDirectoryId, int newParentId)
        {
            var moveResult = _directoryService.MoveDirectory(childDirectoryId, newParentId);

            return ApiResult(moveResult);
        }


        [HttpGet("[controller]/BaseDirectories")]
        public IActionResult GetBaseDirectories()
        {
            var directoriesResult = _dbHelper.GetEntitiesByQuery(_directoryService.GetDirectChildDirectoriesQuery(_directoryService.GetMainDirectoryRootId()));

            return ApiResult(directoriesResult);
        }


        [HttpGet("[controller]/{id}/ChildDirectories")]
        public IActionResult GetDirectChildDirectories(int id)
        {
            var directoriesResult = _dbHelper.GetObjectsByQuery<Tags,DirectoryDetail>(_directoryService.GetDirectChildDirectoriesQuery(id));

            if (directoriesResult.ReturnCode == ReturnCode.NoEntitiesMatchQuery)
            {
                directoriesResult.ReturnCode = ReturnCode.Success;
            }

            return ApiResult(directoriesResult);
        }


        [HttpDelete("[controller]/HardDelete/{id}")]
        public IActionResult HardDelete(int id)
        {
            var childrenResponse = _dbHelper.GetEntitiesByQuery(_directoryService.GetAllChildDirectoriesQuery(id));

            if (childrenResponse.Success)
            {
                if (childrenResponse.Data.Any())
                {
                    return BadRequest($"Directory with id {id} contains child directories. Remove them first");
                }

                return ApiResult(_dbHelper.RemoveEntity<Tags>(id));
            }
            else if (childrenResponse.ReturnCode == ReturnCode.NoEntitiesMatchQuery)
            {
                return ApiResult(_dbHelper.RemoveEntity<Tags>(id));
            }

            return ApiResult(childrenResponse);
        }


        [HttpPost("[controller]/SoftDelete/{id}")]
        public IActionResult SoftDelete(int id)
        {
            var childrenResponse = _dbHelper.GetEntitiesByQuery(_directoryService.GetAllChildDirectoriesQuery(id));

            if (childrenResponse.Success)
            {
                if (childrenResponse.Data.Any())
                {
                    return BadRequest($"Directory with id {id} contains child directories. Remove them first");
                }

                return ApiResult(_dbHelper.UpdateObject<Tags>(id, _tagService.UpdateSoftDelete));
            }
            else if (childrenResponse.ReturnCode == ReturnCode.NoEntitiesMatchQuery)
            {
                return ApiResult(_dbHelper.UpdateObject<Tags>(id, _tagService.UpdateSoftDelete));
            }

            return ApiResult(childrenResponse);
        }
    }
}