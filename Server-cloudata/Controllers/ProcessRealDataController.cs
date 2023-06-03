using BuisnessLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Controllers.Queries;
using System.Collections.Generic;
using System;

namespace Server_cloudata.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProcessRealDataController : Controller
    {
        [HttpGet("ProportionalResidentMemory")]
        public IActionResult NetworkTransmitBytes([FromQuery] QueriesParam queries, [FromQuery] DateTime Start, [FromQuery] string GroupName)    //query="ramusage" start end                  todo to handle clients
        {
            return CommonRequest(queries, Start, GroupName);
        }
        [HttpGet("readBytes")]
        public IActionResult ReadBytes([FromQuery] QueriesParam queries, [FromQuery] DateTime Start, [FromQuery] string GroupName)    //query="ramusage" start end                  todo to handle clients
        {
            return CommonRequest(queries, Start, GroupName);
        }
        public IActionResult CommonRequest(QueriesParam queries, DateTime Start,string GroupName)
        {
            try
            {
                queries.CheckValidation();
                Machine machine = Machine.MachineInstance;
                LinkedList<DataPoint> data = machine.GetData(queries.Exporter, queries.Query, Start, GroupName);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            }
            catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }
    }
}
