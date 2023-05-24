using BuisnessLogic.Collector;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Server_cloudata.Controllers
{
    [Route("api")]
    [ApiController]
    public class VmdataController : ControllerBase
    {
        //ProcessExporter agent = new ProcessExporter();
        //// GET: api/TodoItems
        //
        [HttpGet("cpu")]
        public IActionResult GetCpuUsage()
        { 
            return Ok("Hello Iris");
        }
        //[HttpGet("memory")]
        //public IActionResult GetMemoryUsage()
        //{       
        //    return Ok(agent.GetMemory());
        //}
    }
}
