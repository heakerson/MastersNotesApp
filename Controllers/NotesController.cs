using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nerdable.DbHelper.Models.Response;
using Nerdable.DbHelper.Services;
using Nerdable.NotesApi.NotesAppEntities;
using Nerdable.NotesApi.Services.NoteService;
using Nerdable.NotesApi.Services.NoteService.Models;
using Nerdable.NotesApi.Services.RelationshipService;
using Nerdable.NotesApi.Services.TagService;
using Nerdable.NotesApi.Services.TagService.Models;

namespace Nerdable.NotesApi.Controllers
{
    [ApiController]
    public class NotesController : ApiBaseController
    {
        private readonly IDbHelper _dbHelper;
        private readonly INoteService _noteService;
        private readonly ITagService _tagService;

        public NotesController(IDbHelper dbHelper, INoteService noteService, ITagService tagService)
        {
            _dbHelper = dbHelper;
            _noteService = noteService;
            _tagService = tagService;
        }

        [HttpPost("[controller]/Create")]
        public IActionResult CreateNote([FromBody]NoteCreationModel model)
        {
            if (model.LastUpdated == null)
            {
                model.LastUpdated = DateTime.Now;
            }

            var noteCreateResponse = _dbHelper.AddObject<NoteCreationModel, Notes>(model);

            //Adding the tagNoteRelationships passed in or defining it as 'homeless'
            if (noteCreateResponse.Success)
            {
                int noteId = noteCreateResponse.Data.NoteId;

                if (model.Tags.Any())
                {
                    var addTagsResponse = _noteService.AddTagNoteRelationships(noteId, model.Tags);
                }
                else
                {
                    var addHomelessTag = _noteService.UpdateHomelessTag(noteId);
                }

                var newNoteResponse = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(_noteService.GetNoteQuery(noteId));

                return ApiResult(newNoteResponse);
            }
            else
            {
                return ApiResult(noteCreateResponse);
            }
        }

        [HttpPost("[controller]/Update")]
        public IActionResult UpdateNote([FromBody]NoteUpdateModel model)
        {
            if (model.LastUpdated == null)
            {
                model.LastUpdated = DateTime.Now;
            }

            var noteQuery = _noteService.GetNoteQuery(model.NoteId);

            var basicUpdateResponse = _dbHelper.UpdateObject(noteQuery, model);

            if (basicUpdateResponse.Success)
            {
                var updatedResponse = _dbHelper.GetObjectByQuery<Notes,NoteUpdateModel>(noteQuery);
                return ApiResult(updatedResponse);
            }
            else
            {
                return ApiResult(basicUpdateResponse);
            }
        }

        [HttpPost("[controller]/{noteId}/RemoveTag/{tagId}")]
        public IActionResult RemoveTag(int noteId, int tagId)
        {
            var removeResponse = _noteService.RemoveTagNoteRelationship(noteId, tagId);

            if (removeResponse.Success)
            {
                var note = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(_noteService.GetNoteQuery(noteId));
                var tagNoteRelationshipsResponse = _dbHelper.GetObjectsByQuery<TagNoteRelationship, TagSummary>(_noteService.GetAllTagNoteRelationshipsQuery(noteId));

                if (tagNoteRelationshipsResponse.Success)
                {
                    note.Data.Tags = tagNoteRelationshipsResponse.Data;
                }

                return ApiResult(note);
            }

            return ApiResult(removeResponse);
        }

        [HttpPost("[controller]/{noteId}/AddTag/{tagId}")]
        public IActionResult AddTag(int noteId, int tagId)
        {
            var createResponse = _noteService.AddTagNoteRelationship(noteId,tagId);

            if (createResponse.Success)
            {
                var note = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(_noteService.GetNoteQuery(noteId));
                var tagNoteRelationshipsResponse = _dbHelper.GetObjectsByQuery<TagNoteRelationship, TagSummary>(_noteService.GetAllTagNoteRelationshipsQuery(noteId));

                if (tagNoteRelationshipsResponse.Success)
                {
                    note.Data.Tags = tagNoteRelationshipsResponse.Data;
                }

                return ApiResult(note);
            }

            return ApiResult(createResponse);
        }

        [HttpGet("[controller]/{id}")]
        public IActionResult GetNote(int id)
        {
            var query = _noteService.GetNoteQuery(id);
            var noteResult = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(query);

            return ApiResult(noteResult);
        }

        [HttpDelete("[controller]/HardDelete/{noteId}")]
        public IActionResult HardDeleteNote(int noteId)
        {
            var removeResponse = _dbHelper.RemoveEntity<Notes>(noteId);

            return ApiResult(removeResponse);
        }

        [HttpPost("[Controller]/SoftDelete/{noteId}")]
        public IActionResult SoftDeleteNote(int noteId)
        {
            var response = _dbHelper.UpdateObject<Notes>(noteId, _noteService.UpdateSoftDelete);

            if (response.Success)
            {
                var query = _noteService.GetNoteQuery(noteId);
                var noteDetailResponse = _dbHelper.GetObjectByQuery<Notes, NoteDetail>(query);

                return ApiResult(noteDetailResponse);
            }

            return ApiResult(response);
        }
    }
}