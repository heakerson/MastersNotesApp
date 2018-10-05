using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Services.Response
{
    public enum ReturnCode
    {
        Success = 1,
        Fail,
        InvalidInput,
        DoesNotExist,
        NoEntitiesMatchQuery,
        MappingFailure,
        DatabaseUpdateFailure,
        DatabaseAddFailure,
        DatabaseRemoveFailure,
        DbSetDoesNotExist
    }
}
