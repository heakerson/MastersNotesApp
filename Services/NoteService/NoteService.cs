using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.RelationshipService;
using Nerdable.NotesApi.Services.TagService.Models;

namespace Nerdable.NotesApi.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly NotesAppContext _database;
        private readonly IDbHelper _dbHelper;
        private readonly IRelationshipService _relationshipService;

        public NoteService(NotesAppContext database, IDbHelper dbHelper, IRelationshipService relationshipService)
        {
            _database = database;
            _dbHelper = dbHelper;
            _relationshipService = relationshipService;
        }

        public Response<NoteDetail> RemoveAllTagNoteRelationShips(int noteId)
        {
            var query = GetAllTagNoteRelationships(noteId);

            var removeResponse = _dbHelper.RemoveEntitiesByQuery(query);
            var getDetailResponse = _dbHelper.GetObject<Notes, NoteDetail>(noteId);

            if (removeResponse.Success)
            {
                return getDetailResponse;
            }
            else
            {
                return Response<NoteDetail>.BuildResponse(getDetailResponse.Data, false, removeResponse.ReturnCode, removeResponse.ReturnMessage);
            }
        }

        public Response<NoteDetail> AddTagNoteRelationships(int noteId, List<TagSummary> tags)
        {
            bool errorOccurred = false;
            bool successOccurred = false;
            string message = "";

            if (tags.Any())
            {
                foreach (TagSummary tag in tags)
                {
                    var tagNoteResponse = _dbHelper.AddObject(new TagNoteRelationship { TagId = tag.TagId, NoteId = noteId });

                    if (!tagNoteResponse.Success)
                    {
                        errorOccurred = true;
                        message += $"Failed to create relationship for tag \"{tag.Title}\" with id {tag.TagId} | ";
                    }
                    else
                    {
                        successOccurred = true;
                    }
                }


                var returnDataResponse = _dbHelper.GetObject<Notes, NoteDetail>(GetNoteQuery(noteId));

                if (successOccurred && errorOccurred)
                {
                    return Response<NoteDetail>.BuildResponse(returnDataResponse.Data, true, ReturnCode.PartialSuccess, $"PartialSuccess: {message}");
                }
                else if (!successOccurred && errorOccurred)
                {
                    return Response<NoteDetail>.BuildResponse(returnDataResponse.Data, true, ReturnCode.DatabaseAddFailure, $"No new TagNoteRelationships were created");
                }
                else
                {
                    return returnDataResponse;
                }
            }
            else
            {
                return Response<NoteDetail>.BuildResponse(null, false, ReturnCode.InvalidInput, "No new relationships were passed in to add");
            }
        }

        public Response<NoteDetail> UpdateTagRelationships(int noteId, List<TagSummary> Tags)
        {
            var removeTagsResponse = RemoveAllTagNoteRelationShips(noteId);

            if (removeTagsResponse.Success)
            {
                var addTagsResponse = AddTagNoteRelationships(noteId, Tags);

                return addTagsResponse;
            }
            else
            {
                return removeTagsResponse;
            }
        }

        public IQueryable<Notes> GetNoteQuery(int noteId)
        {
            return _database.Notes
                .Include(note => note.TagNoteRelationship)
                    .ThenInclude(tnr => tnr.Tag)
                .Include(note => note.CreatedByUser)
                .Where(note => note.NoteId == noteId);
        }

        public IQueryable<TagNoteRelationship> GetAllTagNoteRelationships(int noteId)
        {
            return _database.TagNoteRelationship
                .Include(rel => rel.Tag)
                .Include(rel => rel.Note)
                .Where(rel => rel.NoteId == noteId);
        }

        public IQueryable<TagNoteRelationship> GetTagNoteRelationship(int noteId, int tagid)
        {
            return _database.TagNoteRelationship
                .Include(rel => rel.Tag)
                .Include(rel => rel.Note)
                .Where(rel => rel.NoteId == noteId && rel.TagId == tagid);
        }

        public Response<Notes> UpdateSoftDelete(Notes entity)
        {
            entity.IsDeleted = true;
            return Response<Notes>.BuildResponse(entity);
        }

    }
}
