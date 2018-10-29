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
        Response<bool> UpdateHomelessTag(int noteId);

        IQueryable<Notes> GetAllNotes();
        IQueryable<Notes> GetNoteQuery(int noteId);
        IQueryable<TagNoteRelationship> GetHomelessTagNoteQuery(int noteId);
        IQueryable<Notes> GetAllNotesUnderDirectory_Query(int directoryId);
        IQueryable<Notes> GetAllNotes_TagFilter_Query(List<int> tags);
        IQueryable<Notes> FilterNotes_TagFilter_Query(List<int> noteId, List<int> tagIds);
        IQueryable<Notes> FilterRelationships_MustHaveAllTagIds(IQueryable<TagNoteRelationship> relationships, List<int> tagFilters);
        IQueryable<Notes> GetAllNotesUnderDirectory_TagFilter_Query(int directoryId, List<int> tags);

        Response<Notes> UpdateSoftDelete(Notes entity);
    }
}
