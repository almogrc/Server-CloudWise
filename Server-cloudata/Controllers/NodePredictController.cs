using BuisnessLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Controllers.Queries;
using System.Collections.Generic;
using System;
using BuisnessLogic.DTO;
using Server_cloudata.DTO;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector;
using Microsoft.AspNetCore.Http;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services.Collector;
using System.Threading.Tasks;
using BuisnessLogic.Algorithms;
using Server_cloudata.Services.Predict;
using Microsoft.AspNetCore.HttpOverrides;

namespace Server_cloudata.Controllers
{
    [Route("api/machine/[controller]")]
    [ApiController]
    public class NodePredictController : Controller
    {
        public NodePredictController(IPredicteService predictService, SSATimeSeriesForecating ssaAlgo, ArimaTimeSeriesForecasting arimaAlgo, IHttpContextAccessor httpContextAccessor, ThresholdsCollector thresholdsCollector)
        {
            _predictService = predictService;
            _ssaAlgo = ssaAlgo;
            _arimaAlgo = arimaAlgo;
            _httpContextAccessor = httpContextAccessor;
            _thresholdsCollector = thresholdsCollector;
        }

        private IPredicteService _predictService;
        private SSATimeSeriesForecating _ssaAlgo;
        private ArimaTimeSeriesForecasting _arimaAlgo;
        private IHttpContextAccessor _httpContextAccessor;
        private ThresholdsCollector _thresholdsCollector;

        private const string network = "Network";
        private const string ramUsage = "RamUsage";
        private const string networkRecBytes = "NetworkRecBytes";
        private const string networkTransmitBytes = "NetworkTransmitBytes";
        private const string cpuUsage = "CPUUsage";


        [HttpPost(cpuUsage)]
        public async Task<IActionResult> CPUUsage([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _thresholdsCollector.AddThresholsToMetric((await _predictService.GetPredictResult(eNodeExporterData.CPUUsage, Request.Headers[ServerUtils.MachineId], _ssaAlgo)), Request.Headers[ServerUtils.MachineId]));
        }
        [HttpPost(ramUsage)]
        public async Task<IActionResult> RamUsage([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _thresholdsCollector.AddThresholsToMetric((await _predictService.GetPredictResult(eNodeExporterData.RamUsage, Request.Headers[ServerUtils.MachineId], _ssaAlgo)), Request.Headers[ServerUtils.MachineId]));
        }
        [HttpPost(network)]
        public async Task<IActionResult> Network([FromBody] QueriesInfo queriesInfo)
        {
            Metric metricRecive = await _predictService.GetPredictResult(eNodeExporterData.NetworkRecBytes, Request.Headers[ServerUtils.MachineId], _ssaAlgo);
            Metric metricTrans = await _predictService.GetPredictResult(eNodeExporterData.NetworkTransmitBytes, Request.Headers[ServerUtils.MachineId], _ssaAlgo);
            return Ok(new Metric[] { metricRecive, metricTrans });
        }
        // [HttpPost(networkRecBytes)]
        // public async Task<IActionResult> NetworkRecive([FromBody] QueriesInfo queriesInfo)
        // {
        //     return Ok(await _collector.GetData(networkRecBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        // }
        // [HttpPost(networkTransmitBytes)]
        // public async Task<IActionResult> NetworkTransmit([FromBody] QueriesInfo queriesInfo)
        // {
        //     return Ok(await _collector.GetData(networkTransmitBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        // }
        // [HttpPost(network)]
        // public async Task<IActionResult> NetworkBytes([FromBody] QueriesInfo queriesInfo)
        // {
        //     Metric metricNetworkTransmit = await _collector.GetData(networkTransmitBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]);
        //     Metric metricNetworkRecive = await _collector.GetData(networkRecBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]);
        //     return Ok(new Metric[] { metricNetworkTransmit, metricNetworkRecive });
        // }

    }
}
