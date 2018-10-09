using Nerdable.DbHelper.Models.Response;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.TagService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.NoteService
{
    public interface INoteService
    {
        Response<List<TagNoteRelationship>> AddTagNoteRelationships(int noteId, List<TagSummary> tags);
        Response<TagNoteRelationship> AddTagNoteRelationship(int noteId, int tagId);
        Response<NoteDetail> RemoveTagNoteRelationship(int noteId, int tagId);
        Response<NoteDetail> RemoveAllTagNoteRelationships(int noteId);
        Response<NoteDetail> UpdateTagRelationships(int noteId, List<TagSummary> Tags);

        IQueryable<Notes> GetNoteQuery(int noteId);
        IQueryable<TagNoteRelationship> GetAllTagNoteRelationshipsQuery(int noteId);
        IQueryable<TagNoteRelationship> GetTagNoteRelationshipQuery(int noteId, int tagid);

        Response<Notes> UpdateSoftDelete(Notes entity);
    }
}
