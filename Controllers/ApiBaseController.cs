using Nerdable.DbHelper.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Nerdable.NotesApi.Controllers
{
    [Route("api/")]
    public class ApiBaseController : ControllerBase
    {
        public IActionResult ApiResult<TResponse>(Response<TResponse> response)
        {
            switch (response.ReturnCode)
            {
                case ReturnCode.Success:
                case ReturnCode.PartialSuccess:
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
