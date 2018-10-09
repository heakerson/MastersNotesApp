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
    }
}
