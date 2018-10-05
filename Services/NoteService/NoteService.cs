using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApp.Api.NotesAppEntities;
using NotesApp.Api.Services.NoteService.Models;

namespace NotesApp.Api.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly NotesAppContext _database;

        public NoteService(NotesAppContext database)
        {
            _database = database;
        }

        public int CreateUpdateNote(NoteDetail note)
        {
            throw new NotImplementedException();
        }

        public NoteDetail GetNote(int noteId)
        {
            throw new NotImplementedException();
        }
    }
}
