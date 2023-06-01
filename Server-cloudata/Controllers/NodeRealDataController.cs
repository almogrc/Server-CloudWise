using BuisnessLogic.DTO;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Controllers.Queries;
using System;

namespace Server_cloudata.Controllers
{
    [Route("api")]
    [ApiController]
    public class NodeRealDataController : Controller
    {
        [HttpGet("RamUsage")]
        public IActionResult RamUsage([FromQuery] QueriesParam queries)    //query="ramusage" start end                  todo to handle clients
        {
            try
            {
                queries.CheckValidation();
                Machine machine = Machine.MachineInstance;
                machine.GetData(queries.Exporter, queries.Query, queries.Start);
                PredictData predict = machine.PredictForcasting();
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(predict));
            }catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }
    }
}
