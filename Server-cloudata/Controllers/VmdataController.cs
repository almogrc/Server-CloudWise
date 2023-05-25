using BuisnessLogic.Collector;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BuisnessLogic.Model;
using BuisnessLogic.DTO;

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
            Machine machine = new Machine();
            machine.CollectInformation();
            LinearRegressionData res = machine.PredictData();
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(res));
        }
        //[HttpGet("memory")]
        //public IActionResult GetMemoryUsage()
        //{       
        //    return Ok(agent.GetMemory());
        //}
    }
}
