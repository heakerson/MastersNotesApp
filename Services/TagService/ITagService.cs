using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.Response;
using NotesApp.Api.Services.TagService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.TagService
{
    public interface ITagService
    {
        Response<TagCreationModel> CreateTag(TagCreationModel tag);
        Response<TagDetail> UpdateTag(TagDetail tag);

        TagDetail GetTag(int tagId);

        IQueryable<Tags> GetTagQuery(int tagId);
        Response<Tags> SoftDeleteUpdate(Tags entity);
    }
}
