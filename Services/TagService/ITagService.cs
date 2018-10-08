using Nerdable.DbHelper.Models.Response;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.TagService.Models;
using System.Linq;

namespace Nerdable.NotesApi.Services.TagService
{
    public interface ITagService
    {
        IQueryable<Tags> GetTagQuery(int tagId);
        Response<Tags> UpdateSoftDelete(Tags entity);
    }
}
