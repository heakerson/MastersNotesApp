using Nerdable.DbHelper.Models.Response;
using Nerdable.NotesApi.NotesAppEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.RelationshipService
{
    public interface IRelationshipService
    {
        Response<TagNoteRelationship> CreateNewTagNoteRelationship(int noteId, int tagId);

        IQueryable<TagNoteRelationship> GetTagNoteRelationshipQuery(int noteId, int tagId);
        IQueryable<TagNoteRelationship> GetAllTagNoteRelationshipsQuery();
        IQueryable<TagNoteRelationship> GetAllTagNotesByNoteId_Query(int noteId);
        IQueryable<TagNoteRelationship> GetAllTagNotesByNoteIds_Query(List<int> noteIds);
        IQueryable<TagNoteRelationship> GetAllTagNotesByTagId_Query(int tagId);
        IQueryable<TagNoteRelationship> GetAllTagNotesByTagIds_Query(List<int> tagId);
        IQueryable<TagNoteRelationship> GetAllTagNotesByTagIds_Query(IQueryable<int> tagId);
    }
}
