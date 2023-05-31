using BuisnessLogic.Collector;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BuisnessLogic.Model;
using BuisnessLogic.Algorithms.DTOPrediction;
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
            PredictData predict = machine.PredictForcasting();
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(predict));
        }
        //[HttpGet("memory")]
        //public IActionResult GetMemoryUsage()
        //{       
        //    return Ok(agent.GetMemory());
        //}
    }
}
