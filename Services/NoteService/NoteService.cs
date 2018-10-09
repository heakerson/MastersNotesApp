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

        public Response<NoteDetail> RemoveTagNoteRelationship(int noteId, int tagId)
        {
            var tagNotQuery = GetTagNoteRelationshipQuery(noteId, tagId);

            var removeResponse = _dbHelper.RemoveEntitiesByQuery(tagNotQuery);

            var noteResponse = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(GetNoteQuery(noteId));

            if (removeResponse.Success)
            {
                return Response<NoteDetail>.BuildResponse(noteResponse.Data);
            }
            else
            {
                return Response<NoteDetail>.BuildResponse(noteResponse.Data, false, removeResponse.ReturnCode, removeResponse.ReturnMessage);
            }
        }

        public Response<NoteDetail> RemoveAllTagNoteRelationships(int noteId)
        {
            var query = GetAllTagNoteRelationshipsQuery(noteId);

            var removeResponse = _dbHelper.RemoveEntitiesByQuery(query);
            var getDetailResponse = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(GetNoteQuery(noteId));

            if (removeResponse.Success)
            {
                return getDetailResponse;
            }
            else
            {
                return Response<NoteDetail>.BuildResponse(getDetailResponse.Data, false, removeResponse.ReturnCode, removeResponse.ReturnMessage);
            }
        }

        public Response<TagNoteRelationship> AddTagNoteRelationship(int noteId, int tagId)
        {
            var relationshipResponse = _relationshipService.CreateNewTagNoteRelationship(noteId, tagId);

            if (relationshipResponse.Success)
            {
                var tagNoteResponse = _dbHelper.AddObject(relationshipResponse.Data);

                return tagNoteResponse;
            }
            else
            {
                return relationshipResponse;
            }
        }

        public Response<List<TagNoteRelationship>> AddTagNoteRelationships(int noteId, List<TagSummary> tags)
        {
            List<TagNoteRelationship> responseData = new List<TagNoteRelationship>();

            bool errorOccurred = false;
            bool successOccurred = false;
            string message = "";



            if (tags.Any())
            {
                foreach (TagSummary tag in tags)
                {
                    var addNewRelationshipResponse = AddTagNoteRelationship(noteId, tag.TagId);

                    if (addNewRelationshipResponse.Success)
                    {
                        successOccurred = true;
                        responseData.Add(addNewRelationshipResponse.Data);
                    }
                    else
                    {
                        if (errorOccurred)
                        {
                            message += $"[ NoteId: {noteId} and TagId: {tag.TagId} Error Message: {addNewRelationshipResponse.ReturnMessage} ] ";
                        }
                        else
                        {
                            message += $"The following tagNoteRelationships were not added: [ NoteId: {noteId} and TagId: {tag.TagId} Error Message: {addNewRelationshipResponse.ReturnMessage} ] ";
                            errorOccurred = true;
                        }
                    }
                }


                if (successOccurred && errorOccurred)
                {
                    return Response<List<TagNoteRelationship>>.BuildResponse(responseData, true, ReturnCode.PartialSuccess, $"PARTIAL SUCCESS: {message}");
                }
                else if (!successOccurred && errorOccurred)
                {
                    return Response<List<TagNoteRelationship>>.BuildResponse(responseData, true, ReturnCode.DatabaseAddFailure, $"No new TagNoteRelationships were created: {message}");
                }
                else
                {
                    return Response<List<TagNoteRelationship>>.BuildResponse(responseData);
                }
            }
            else
            {
                return Response<List<TagNoteRelationship>>.BuildResponse(responseData, false, ReturnCode.InvalidInput, "No new tag note relationships were passed in to add");
            }
        }

        public Response<NoteDetail> UpdateTagRelationships(int noteId, List<TagSummary> Tags)
        {
            var removeTagsResponse = RemoveAllTagNoteRelationships(noteId);

            if (removeTagsResponse.Success)
            {
                var addTagsResponse = AddTagNoteRelationships(noteId, Tags);

                var detailResponse = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(GetNoteQuery(noteId));

                return Response<NoteDetail>.BuildResponse(detailResponse.Data, addTagsResponse.Success, addTagsResponse.ReturnCode, addTagsResponse.ReturnMessage);
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

        public IQueryable<TagNoteRelationship> GetAllTagNoteRelationshipsQuery(int noteId)
        {
            return _database.TagNoteRelationship
                .Include(rel => rel.Tag)
                .Include(rel => rel.Note)
                .Where(rel => rel.NoteId == noteId);
        }

        public IQueryable<TagNoteRelationship> GetTagNoteRelationshipQuery(int noteId, int tagid)
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
