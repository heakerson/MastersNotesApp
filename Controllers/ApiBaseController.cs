using Microsoft.AspNetCore.Mvc;
using NotesApp.Api.Services.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Api.Controllers
{
    [Route("api/")]
    public class ApiBaseController : ControllerBase
    {
        public IActionResult ApiResult<TResponse>(Response<TResponse> response)
        {
            switch (response.ReturnCode)
            {
                case ReturnCode.Success:
                    return Ok(response);
                case ReturnCode.DoesNotExist:
                case ReturnCode.NoEntitiesMatchQuery:
                    return NotFound(response);
                case ReturnCode.DatabaseAddFailure:
                case ReturnCode.DatabaseUpdateFailure:
                case ReturnCode.DatabaseRemoveFailure:
                case ReturnCode.MappingFailure:
                case ReturnCode.Fail:
                    return StatusCode(500, response);
                case ReturnCode.InvalidInput:
                default:
                    return BadRequest(response);
            }
        }
    }
}
