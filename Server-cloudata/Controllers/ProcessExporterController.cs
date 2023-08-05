using BuisnessLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Controllers.Queries;
using System.Collections.Generic;
using System;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector;
using Microsoft.AspNetCore.Http;
using Server_cloudata.Services.Collector;
using Server_cloudata.DTO;
using Server_cloudata.ServerDataManager;

namespace Server_cloudata.Controllers
{
    [Route("api/machine/[controller]")]
    [ApiController]
    public class ProcessExporterController : ControllerBase
    {
        public ProcessExporterController(IProcessExporterService<List<Metric>> collectorService, IHttpContextAccessor httpContextAccessor)
        {
            _collectorService = collectorService;
            _httpContextAccessor = httpContextAccessor;
        }

        IProcessExporterService<List<Metric>> _collectorService;
        private IHttpContextAccessor _httpContextAccessor;
        private const string proportionalResidentMemory = "ProportionalResidentMemory";
       
        [HttpPost(proportionalResidentMemory)]
        public IActionResult NetworkTransmitBytes([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(_collectorService.GetData(proportionalResidentMemory, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
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
               // queries.CheckValidation();
               // Machine machine = Machine.MachineInstance;
               // LinkedList<DataPoint> data = machine.GetData(queries.Exporter, queries.Query, Start, GroupName);
               return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(""));
            }
            catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }
    }
}
