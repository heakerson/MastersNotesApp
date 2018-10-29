using System.Linq;
using Nerdable.DbHelper.Services;
using Microsoft.AspNetCore.Mvc;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.TagService;
using Nerdable.NotesApi.Services.TagService.Models;
using Nerdable.NotesApi.Services.Search;

namespace Nerdable.NotesApi.Controllers
{
    [ApiController]
    public class TagController : ApiBaseController
    {
        private readonly ITagService _tagService;
        private readonly IDbHelper _dbHelper;

        public TagController(ITagService tagService, IDbHelper dbHelper)
        {
            _tagService = tagService;
            _dbHelper = dbHelper;
        }

        [HttpPost("[Controller]/Create")]
        public IActionResult CreateTag([FromBody]TagCreationModel model)
        {
            var response = _dbHelper.AddObject<TagCreationModel, Tags>(model);

            return ApiResult(response);
        }


        [HttpPost("[Controller]/Update")]
        public IActionResult UpdateTag([FromBody]TagUpdateModel model)
        {
            var updateResponse = _dbHelper.UpdateObject<TagUpdateModel, Tags>(model, model.TagId);

            return ApiResult(updateResponse);
        }

        [HttpGet("[Controller]/All")]
        public IActionResult GetAllTags()
        {
            IQueryable<Tags> query = _tagService.GetAllTagsQuery();
            var response = _dbHelper.GetObjectsByQuery<Tags, TagSummary>(query);

            return ApiResult(response);
        }


        [HttpGet("[Controller]/{tagId}")]
        public IActionResult GetTag(int tagId)
        {
            IQueryable<Tags> query = _tagService.GetTagQuery(tagId);
            var response = _dbHelper.GetObjectByQuery<Tags, TagDetail>(query);

            return ApiResult(response);
        }

        [HttpPost("[Controller]/Search")]
        public IActionResult SearchByTitle([FromBody]SearchBase search)
        {
            IQueryable<Tags> query = _tagService.GetTagsBySearch_Query(search.SearchTerm);
            var response = _dbHelper.GetObjectsByQuery<Tags, TagSummary>(query);

            return ApiResult(response);
        }


        [HttpDelete("[Controller]/HardDelete/{tagId}")]
        public IActionResult HardDeleteTag(int tagId)
        {
            var removeResponse = _dbHelper.RemoveEntity<Tags>(tagId);

            return ApiResult(removeResponse);
        }


        [HttpPost("[Controller]/SoftDelete/{tagId}")]
        public IActionResult SoftDeleteTag(int tagId)
        {
            var response = _dbHelper.UpdateObject<Tags>(tagId, _tagService.UpdateSoftDelete);

            return ApiResult(response);
        }
    }
}