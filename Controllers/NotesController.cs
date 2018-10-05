using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ApiBaseController
    {

        [HttpGet("{id}")]
        public IActionResult GetNote(int id)
        {
            return Ok($"You passed in id: {id}");
        }
    }
}