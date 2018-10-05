using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.DatabaseServices;
using NotesApp.Api.Services.Response;
using NotesApp.Api.Services.TagService;
using NotesApp.Api.Services.TagService.Models;

namespace NotesApp.Api.Controllers
{
    [ApiController]
    public class TagController : ApiBaseController
    {
        private readonly ITagService _tagService;
        private readonly IDatabaseService _databaseService;
        private readonly NotesAppContext _context;

        public TagController(ITagService tagService, IDatabaseService databaseService, NotesAppContext context)
        {
            _tagService = tagService;
            _databaseService = databaseService;
            _context = context;
        }

        [HttpPost("[Controller]/Create")]
        public IActionResult CreateTag([FromBody]TagCreationModel model)
        {
            var response = _databaseService.AddObject<TagCreationModel, Tags>(model);

            return ApiResult(response);
        }

        [HttpPost("[Controller]/Update")]
        public IActionResult UpdateTag([FromBody]TagUpdateModel model)
        {
            var updateResponse = _databaseService.UpdateObject<TagUpdateModel, Tags>(model, model.TagId);

            return ApiResult(updateResponse);
        }


        [HttpGet("[Controller]/{tagId}")]
        public IActionResult GetTag(int tagId)
        {
            IQueryable<Tags> query = _tagService.GetTagQuery(tagId);
            var response = _databaseService.GetObject<Tags, TagDetail>(query);

            return ApiResult(response);
        }


        [HttpDelete("[Controller]/HardDelete/{tagId}")]
        public IActionResult HardDeleteTag(int tagId)
        {
            var response = _databaseService.RemoveEntity<Tags>(tagId);

            return ApiResult(response);
        }


        [HttpDelete("[Controller]/SoftDelete/{tagId}")]
        public IActionResult SoftDeleteTag(int tagId)
        {
            var response = _databaseService.UpdateObject<Tags>(tagId, _tagService.SoftDeleteUpdate);

            return ApiResult(response);
        }
    }
}