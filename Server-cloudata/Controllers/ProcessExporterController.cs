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
using System.Threading.Tasks;

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
        private const string residentMemory = "ResidentMemory";
        private const string cpuUser = "CpuUser";
        private const string cpuSystem = "CpuSystem";
        private const string readBytes = "ReadBytes";

        [HttpPost(residentMemory)]
        public async Task<IActionResult> ResidentMemory([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collectorService.GetData(residentMemory, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }

        [HttpPost(cpuUser)]
        public async Task<IActionResult> CPUUser([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collectorService.GetData(cpuUser, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }
        [HttpPost(cpuSystem)]
        public async Task<IActionResult> CPUSystem([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collectorService.GetData(cpuSystem, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }

        [HttpPost(readBytes)]
        public async Task<IActionResult> ReadBytes([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collectorService.GetData(readBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }
    }
}
