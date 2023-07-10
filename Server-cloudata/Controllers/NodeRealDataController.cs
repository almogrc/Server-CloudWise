using BuisnessLogic.DTO;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Controllers.Queries;
using System;
using System.Collections.Generic;

namespace Server_cloudata.Controllers
{
    [Route("api")]
    [ApiController]
    public class NodeRealDataController : Controller
    {
        [HttpGet("RamUsage")]
        public IActionResult RamUsage([FromQuery] QueriesParam queries, [FromQuery] DateTime Start)    //query="ramusage" start end                  todo to handle clients
        {
           return CommonRequest(queries, Start);
        }
        [HttpGet("NetworkRecBytes")]
        public IActionResult NetworkReciveBytes([FromQuery] QueriesParam queries, [FromQuery] DateTime Start)    //query="ramusage" start end                  todo to handle clients
        {
            return CommonRequest(queries, Start);
        }
        [HttpGet("NetworkTransmitBytes")]
        public IActionResult NetworkTransmitBytes([FromQuery] QueriesParam queries, [FromQuery] DateTime Start)    //query="ramusage" start end                  todo to handle clients
        {
            return CommonRequest(queries, Start);
        }
        public IActionResult CommonRequest(QueriesParam queries, DateTime Start)
        {
            try
            {
                queries.CheckValidation();
                Machine machine = Machine.MachineInstance;
                LinkedList<DataPoint> data = machine.GetData(queries.Exporter, queries.Query, Start);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            }
            catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }
    }
}
