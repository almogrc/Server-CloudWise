using BuisnessLogic.Algorithms;
using BuisnessLogic.Collector.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.DTO;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services.Collector;
using Server_cloudata.Services.Predict;
using System.Threading.Tasks;

namespace Server_cloudata.Controllers
{
    [Route("api/machine/[controller]")]
    public class ProcessPredictController : Controller
    {
        public ProcessPredictController(IPredicteService predictService, SSATimeSeriesForecating ssaAlgo, ArimaTimeSeriesForecasting arimaAlgo, IHttpContextAccessor httpContextAccessor)
        {
            _predictService = predictService;
            _ssaAlgo = ssaAlgo;
            _arimaAlgo = arimaAlgo;
            _httpContextAccessor = httpContextAccessor;
        }

        private IPredicteService _predictService;
        private SSATimeSeriesForecating _ssaAlgo;
        private ArimaTimeSeriesForecasting _arimaAlgo;
        private IHttpContextAccessor _httpContextAccessor;

        private const string residentMemory = "ResidentMemory";
        private const string cpuUser = "CpuUser";
        private const string cpuSystem = "CpuSystem";
        private const string readBytes = "ReadBytes";

        [HttpPost(residentMemory)]
        public async Task<IActionResult> ResidentMemory([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _predictService.GetPredictResult(eProcessExporterData.ResidentMemory, Request.Headers[ServerUtils.MachineId], _ssaAlgo));
        }

        [HttpPost(cpuUser)]
        public async Task<IActionResult> CPUUser([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _predictService.GetPredictResult(eProcessExporterData.CpuUser, Request.Headers[ServerUtils.MachineId], _ssaAlgo));
        }
        [HttpPost(cpuSystem)]
        public async Task<IActionResult> CPUSystem([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _predictService.GetPredictResult(eProcessExporterData.CpuSystem, Request.Headers[ServerUtils.MachineId], _ssaAlgo));
        }

        [HttpPost(readBytes)]
        public async Task<IActionResult> ReadBytes([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _predictService.GetPredictResult(eProcessExporterData.ReadBytes, Request.Headers[ServerUtils.MachineId], _ssaAlgo));
        }


    }
}
