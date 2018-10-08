﻿using System;
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
using Nerdable.NotesApi.Services.TagService.Models;

namespace Nerdable.NotesApi.Controllers
{
    [ApiController]
    public class NotesController : ApiBaseController
    {
        private readonly IDbHelper _dbHelper;
        private readonly IMapper _mapper;
        private readonly INoteService _noteService;

        public NotesController(IDbHelper dbHelper, IMapper mapper, INoteService noteService)
        {
            _dbHelper = dbHelper;
            _mapper = mapper;
            _noteService = noteService;
        }

        [HttpPost("[controller]/Create")]
        public IActionResult CreateNote([FromBody]NoteCreationModel model)
        {
            if (model.LastUpdated == null)
            {
                model.LastUpdated = DateTime.Now;
            }

            var createResponse = _dbHelper.AddObject<NoteCreationModel, Notes>(model);


            if (createResponse.Success)
            {
                int noteId = createResponse.Data.NoteId;

                if (model.Tags.Any())
                {
                    var addTagsResponse = _noteService.AddTagNoteRelationships(noteId, model.Tags);

                    return ApiResult(addTagsResponse);
                }
                else
                {
                    var homelessTagResponse = _dbHelper.GetObject<Tags, TagSummary>(20);

                    List<TagSummary> homelessTag = new List<TagSummary>();
                    homelessTag.Add(homelessTagResponse.Data);

                    var addHomelessTag = _noteService.AddTagNoteRelationships(noteId, homelessTag);
                    return ApiResult(addHomelessTag);
                }
            }
            else
            {
                return ApiResult(createResponse);
            }
        }

        [HttpPost("[controller]/Update")]
        public IActionResult UpdateNote([FromBody]NoteUpdateModel model)
        {
            if (model.LastUpdated == null)
            {
                model.LastUpdated = DateTime.Now;
            }

            var basicUpdateResponse = _dbHelper.UpdateObject<NoteUpdateModel, Notes>(model, model.NoteId);

            if (basicUpdateResponse.Success)
            {
                var updateTagsResponse = _noteService.UpdateTagRelationships(model.NoteId, model.Tags);

                return ApiResult(updateTagsResponse);
            }
            else
            {
                return ApiResult(basicUpdateResponse);
            }
        }

        [HttpPost("[controller]/{noteId}/RemoveTag/{tagId}")]
        public IActionResult RemoveTag(int noteId, int tagId)
        {
            var query = _noteService.GetTagNoteRelationship(noteId,tagId);

            var removeResponse = _dbHelper.RemoveEntitiesByQuery(query);

            if (removeResponse.Success)
            {
                var noteQuery = _noteService.GetNoteQuery(noteId);
                return ApiResult(_dbHelper.GetObject<Notes, NoteDetail>(noteQuery));
            }

            return ApiResult(removeResponse);
        }

        [HttpPost("[controller]/{noteId}/AddTag/{tagId}")]
        public IActionResult AddTag(int noteId, int tagId)
        {
            var createResponse = _dbHelper.AddObject(new TagNoteRelationship { NoteId = noteId, TagId = tagId});

            if (createResponse.Success)
            {
                var noteQuery = _noteService.GetNoteQuery(noteId);
                return ApiResult(_dbHelper.GetObject<Notes, NoteDetail>(noteQuery));
            }

            return ApiResult(createResponse);
        }

        [HttpGet("[controller]/{id}")]
        public IActionResult GetNote(int id)
        {
            var query = _noteService.GetNoteQuery(id);
            return ApiResult(_dbHelper.GetObject<Notes,NoteDetail>(query));
        }

        [HttpDelete("[controller]/HardDelete/{noteId}")]
        public IActionResult HardDeleteNote(int noteId)
        {
            return ApiResult(_dbHelper.RemoveEntity<Notes>(noteId));
        }

        [HttpPost("[Controller]/SoftDelete/{noteId}")]
        public IActionResult SoftDeleteNote(int noteId)
        {
            var response = _dbHelper.UpdateObject<Notes>(noteId, _noteService.UpdateSoftDelete);

            if (response.Success)
            {
                var detail = _dbHelper.GetObject<Notes, NoteDetail>(noteId);
                return ApiResult(detail);
            }

            return ApiResult(response);
        }
    }
}