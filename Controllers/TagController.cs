using System.Linq;
using Nerdable.DbHelper.Services;
using Microsoft.AspNetCore.Mvc;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.TagService;
using Nerdable.NotesApi.Services.TagService.Models;

namespace Nerdable.NotesApi.Controllers
{
    [ApiController]
    public class TagController : ApiBaseController
    {
        private readonly ITagService _tagService;
        private readonly IDbHelper _databaseService;
        private readonly NotesAppContext _context;

        public TagController(ITagService tagService, IDbHelper databaseService, NotesAppContext context)
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
            var response = _databaseService.GetObjectByQuery<Tags, TagDetail>(query);

            return ApiResult(response);
        }


        [HttpDelete("[Controller]/HardDelete/{tagId}")]
        public IActionResult HardDeleteTag(int tagId)
        {
            var removeResponse = _databaseService.RemoveEntity<Tags>(tagId);

            return ApiResult(removeResponse);
        }


        [HttpPost("[Controller]/SoftDelete/{tagId}")]
        public IActionResult SoftDeleteTag(int tagId)
        {
            var response = _databaseService.UpdateObject<Tags>(tagId, _tagService.UpdateSoftDelete);

            return ApiResult(response);
        }
    }
}