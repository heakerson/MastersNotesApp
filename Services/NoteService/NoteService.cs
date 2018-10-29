using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.DirectoryService;
using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.RelationshipService;
using Nerdable.NotesApi.Services.TagService;
using Nerdable.NotesApi.Services.TagService.Models;

namespace Nerdable.NotesApi.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly NotesAppContext _database;
        private readonly IDbHelper _dbHelper;
        private readonly IRelationshipService _relationshipService;
        private readonly ITagService _tagService;
        private readonly IDirectoryService _directoryService;

        public NoteService(NotesAppContext database, IDbHelper dbHelper, IRelationshipService relationshipService, ITagService tagService, IDirectoryService directoryService)
        {
            _database = database;
            _dbHelper = dbHelper;
            _relationshipService = relationshipService;
            _tagService = tagService;
            _directoryService = directoryService;
        }

        public Response<NoteDetail> RemoveTagNoteRelationship(int noteId, int tagId)
        {
            var tagNotQuery = _relationshipService.GetTagNoteRelationshipQuery(noteId, tagId);

            var removeResponse = _dbHelper.RemoveEntitiesByQuery(tagNotQuery);

            var updateHomelessResponse = UpdateHomelessTag(noteId);

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
            var query = _relationshipService.GetAllTagNotesByNoteId_Query(noteId);

            var removeResponse = _dbHelper.RemoveEntitiesByQuery(query);
            var getDetailResponse = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(GetNoteQuery(noteId));

            if (removeResponse.Success)
            {
                var homelessUpdateResponse = UpdateHomelessTag(noteId);
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

                var updateHomelessResponse = UpdateHomelessTag(noteId);

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

            if (tags.Any())
            {
                bool errorOccurred = false;
                bool successOccurred = false;
                string message = "";

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

                var homelessUpdateResponse = UpdateHomelessTag(noteId);

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

                //var homelessUpdate = UpdateHomelessTag(noteId);

                return Response<NoteDetail>.BuildResponse(detailResponse.Data, addTagsResponse.Success, addTagsResponse.ReturnCode, addTagsResponse.ReturnMessage);
            }
            else
            {
                return removeTagsResponse;
            }
        }

        public Response<bool> UpdateHomelessTag(int noteId)
        {
            var query = _relationshipService.GetAllTagNotesByNoteId_Query(noteId);
            var tagNoteResponse = _dbHelper.GetEntitiesByQuery(query);
            int homelessTagId = _tagService.GetHomelessTagId();

            if (tagNoteResponse.ReturnCode == ReturnCode.NoEntitiesMatchQuery)
            {
                //Adding the homeless tag if the note has no other tags
                var homelessTagRelationshipCreate = _relationshipService.CreateNewTagNoteRelationship(noteId, homelessTagId);
                var homelessTagRelationshipAdd = _dbHelper.AddObject(homelessTagRelationshipCreate.Data);

                return Response<bool>.BuildResponse(true);
            }
            else
            {
                //Removing the homeless tag if there are other tags associated with the note
                var homelessTagRelationship = tagNoteResponse.Data.Where(t => t.TagId == homelessTagId).FirstOrDefault();

                if (homelessTagRelationship != null && tagNoteResponse.Data.Count() > 1)
                {
                    //atabase.Entry(homelessTagRelationship).State = EntityState.Detached;
                    var removeResponse = _dbHelper.RemoveEntitiesByQuery(GetHomelessTagNoteQuery(noteId));
                    return Response<bool>.BuildResponse(true);
                }

            }

            return Response<bool>.BuildResponse(false);
        }

        public IQueryable<Notes> GetAllNotes()
        {
            return _database.Notes
                .Include(note => note.TagNoteRelationship)
                    .ThenInclude(tnr => tnr.Tag)
                .Include(note => note.CreatedByUser);
        }

        public IQueryable<Notes> GetNoteQuery(int noteId)
        {
            return GetAllNotes()
                .Where(note => note.NoteId == noteId);
        }



        public IQueryable<TagNoteRelationship> GetHomelessTagNoteQuery(int noteId)
        {
            int homelessTagId = _tagService.GetHomelessTagId();

            return _relationshipService.GetTagNoteRelationshipQuery(noteId, homelessTagId);
        }

        public IQueryable<Notes> GetAllNotes_TagFilter_Query(List<int> tagFilter)
        {
            var relationships = _relationshipService.GetAllTagNotesByTagIds_Query(tagFilter);

            var testRelation = relationships.ToList();

            return FilterRelationships_MustHaveAllTagIds(relationships, tagFilter);


            //return _relationshipService.GetAllTagNotesByTagIds_Query(tagFilter)
            //    .GroupBy(tr => tr.NoteId)
            //    .Select(g => new NoteGrouping_SelectTags
            //    {
            //        NoteId = g.Key,
            //        Note = g.Select(tr => tr.Note).FirstOrDefault(),
            //        TagIds = g.Select(tr => tr.TagId)
            //    })
            //    .Where(ng => ng.TagIds.Intersect(tagFilter).Equals(tagFilter))
            //    .Select(g => g.Note);
        }

        public IQueryable<Notes> FilterNotes_TagFilter_Query(List<int> noteIds, List<int> tagIds)
        {
            var relationships = _relationshipService.GetAllTagNotesByNoteIds_Query(noteIds);

            return FilterRelationships_MustHaveAllTagIds(relationships, tagIds);

            ////return _relationshipService.GetAllTagNotesByNoteIds_Query(noteIds)
            ////    .GroupBy(tr => tr.NoteId)
            ////    .Select(g => new NoteGrouping_SelectTags
            ////    {
            ////        NoteId = g.Key,
            ////        Note = g.Select(tr => tr.Note).FirstOrDefault(),
            ////        TagIds = g.Select(tr => tr.TagId)
            ////    })
            ////    .Where(ng => ng.TagIds.Intersect(tagIds).Equals(tagIds))
            ////    .Select(g => g.Note);
        }

        public IQueryable<Notes> FilterRelationships_MustHaveAllTagIds(IQueryable<TagNoteRelationship> relationships, List<int> tagFilters)
        {
            return relationships
                .GroupBy(tr => tr.NoteId)
                .Select(g => new NoteGrouping_SelectTags
                {
                    NoteId = g.Key,
                    Note = g.Select(tr => tr.Note).FirstOrDefault(),
                    TagIds = g.Select(tr => tr.TagId)
                })
                .Where(ng => ParentContainsChild(ng.TagIds, tagFilters))
                .Select(g => g.Note);
        }

        private bool ParentContainsChild(IEnumerable<int> parent, IEnumerable<int> child)
        {
            foreach (int id in child)
            {
                if (!parent.Contains(id))
                {
                    return false;
                }
            }

            return true;
            var intersection = parent.ToList().Intersect(child.ToList()).ToList();
            var equals = intersection.Equals(child);

            return equals;
        }

        public IQueryable<Notes> GetAllNotesUnderDirectory_TagFilter_Query(int directoryId, List<int> tagFilter)
        {
            var noteIdsUnderDirectory = GetAllNotesUnderDirectory_Query(directoryId)
                .Select(n => n.NoteId)
                .ToList();

            return FilterNotes_TagFilter_Query(noteIdsUnderDirectory, tagFilter);
        }

        public IQueryable<Notes> GetAllNotesUnderDirectory_Query(int directoryId)
        {
            var childDirectoryIds = _directoryService.GetAllChildDirectoriesQuery(directoryId)
                .Select(d => d.TagId);

            return _relationshipService.GetAllTagNoteRelationshipsQuery()
                .Where(tr => childDirectoryIds.Contains(tr.TagId) || tr.TagId == directoryId)
                //.Select(tr => tr.Note)
                .GroupBy(g => g.NoteId)
                .Select(g => g.Select(tr => tr.Note).FirstOrDefault());

            return _relationshipService.GetAllTagNoteRelationshipsQuery()
                .Where(tr => childDirectoryIds.Contains(tr.TagId) || tr.TagId == directoryId)
                .Select(tr => tr.Note);
        }

        public Response<Notes> UpdateSoftDelete(Notes entity)
        {
            entity.IsDeleted = true;
            return Response<Notes>.BuildResponse(entity);
        }

    }
}
