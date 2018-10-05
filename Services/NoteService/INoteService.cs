using NotesApp.Api.Services.NoteService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.NoteService
{
    public interface INoteService
    {
        int CreateUpdateNote(NoteDetail note);
        NoteDetail GetNote(int noteId);
    }
}
