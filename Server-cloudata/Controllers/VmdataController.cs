using BuisnessLogic.Requester;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VmdataController : ControllerBase
    {
        AgentApi agent = new NodeExporter();
        // GET: api/TodoItems
       
        [HttpGet]
        [Route("[cpu]")]
        public async Task<string> GetCpuUsage()
        {
            Dictionary<string, int> points = new Dictionary<string, int>
            {
                { "James", 9001 },
                { "Jo", 3474 },
                { "Jess", 11926 }
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(points);
        }
    }
}
