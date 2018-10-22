using Microsoft.EntityFrameworkCore;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdable.NotesApi.Services.RelationshipService
{
    public class RelationshipService : IRelationshipService
    {
        private readonly NotesAppContext _database;
        private readonly IDbHelper _dbHelper;

        public RelationshipService(NotesAppContext database, IDbHelper dbHelper)
        {
            _database = database;
            _dbHelper = dbHelper;
        }

        public IQueryable<TagNoteRelationship> GetAllTagNoteRelationshipsQuery()
        {
            return _database.TagNoteRelationship
                .Include(rel => rel.Tag)
                .Include(rel => rel.Note)
                    .ThenInclude(n => n.CreatedByUser);
        }

        public IQueryable<TagNoteRelationship> GetTagNoteRelationshipQuery(int noteId, int tagid)
        {
            return GetAllTagNoteRelationshipsQuery()
                    .Where(rel => rel.TagId == tagid && rel.NoteId == noteId);
        }

        //public IQueryable<TagNoteRelationship> GetAllTagNotesByNoteIds_Query(List<int> noteIds)
        //{
        //    return GetAllTagNoteRelationshipsQuery()
        //        .Where(rel => noteIds.Contains(rel.NoteId));
        //}

        //public IQueryable<TagNoteRelationship> GetAllTagNotesByTagIds_Query(List<int> tagIds)
        //{

        //}

        public IQueryable<TagNoteRelationship> GetAllTagNotesByNoteId_Query(int noteId)
        {
            return GetAllTagNoteRelationshipsQuery()
                .Where(rel => rel.NoteId == noteId);
        }

        public IQueryable<TagNoteRelationship> GetAllTagNotesByTagId_Query(int tagId)
        {
            return GetAllTagNoteRelationshipsQuery()
                .Where(rel => rel.TagId == tagId);
        }

        public Response<TagNoteRelationship> CreateNewTagNoteRelationship(int noteId, int tagId)
        {
            var noteResponse = _dbHelper.GetEntity<Notes>(noteId);

            if (noteResponse.Success)
            {
                var tagResponse = _dbHelper.GetEntity<Tags>(tagId);

                if (tagResponse.Success)
                {
                    var relationship = new TagNoteRelationship() { Tag = tagResponse.Data, Note = noteResponse.Data, TagId = tagId, NoteId = noteId };
                    return Response<TagNoteRelationship>.BuildResponse(relationship);
                }
                else
                {
                    return Response<TagNoteRelationship>.BuildResponse(null, false, tagResponse.ReturnCode, tagResponse.ReturnMessage);
                }
            }
            else
            {
                return Response<TagNoteRelationship>.BuildResponse(null, false, noteResponse.ReturnCode, noteResponse.ReturnMessage);
            }
        }
    }
}
