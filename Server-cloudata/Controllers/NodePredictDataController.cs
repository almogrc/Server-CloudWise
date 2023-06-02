using BuisnessLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Controllers.Queries;
using System.Collections.Generic;
using System;
using BuisnessLogic.DTO;

namespace Server_cloudata.Controllers
{
    [Route("api")]
    [ApiController]
    public class NodePredictDataController : Controller
    {
        [HttpGet("RamUsagePredict")]
        public IActionResult RamUsage(/*[FromQuery] QueriesParam queries*/)    //query="ramusage" start end                  todo to handle clients
        {
            try
            {
               // queries.CheckValidation();
                Machine machine = Machine.MachineInstance;
                //PredictData data = machine.PredictForcasting();
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(""));
            }
            catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }
    }
}
