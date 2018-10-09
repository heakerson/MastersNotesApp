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
