using BuisnessLogic.Requester;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Server_cloudata.Controllers
{
    [Route("api")]
    [ApiController]
    public class VmdataController : ControllerBase
    {
        AgentApi agent = new NodeExporter();
        // GET: api/TodoItems

        [HttpGet("cpu")]
        public IActionResult GetCpuUsage()
        { 
            return Ok(agent.GetCpu());
        }
        [HttpGet("memory")]
        public IActionResult GetMemoryUsage()
        {       
            return Ok(agent.GetMemory());
        }
    }
}
