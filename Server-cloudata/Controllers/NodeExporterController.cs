using BuisnessLogic.Collector;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Extentions;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Server_cloudata.DTO;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services.Collector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata.Controllers
{
    [Route("api/machine/[controller]")]
    [ApiController]
    public class NodeExporterController : ControllerBase
    {
        public NodeExporterController(INodeCollectorService<Metric> collector, ICollector<eNodeExporterData> collectorNodeExporter, IHttpContextAccessor httpContextAccessor)
        {
            _collector = collector;
            _httpContextAccessor = httpContextAccessor;
            _collectorNodeExporter = collectorNodeExporter;
        }

        private INodeCollectorService<Metric> _collector;
        ICollector<eNodeExporterData> _collectorNodeExporter;
        private const string network = "Network";
        private const string ramUsage = "RamUsage";
        private const string networkRecBytes = "NetworkRecBytes";
        private const string networkTransmitBytes = "NetworkTransmitBytes";
        private const string cpuUsage = "CPUUsage";
        private const string cpuGauge = "CPUGauge";
        private const string ramGauge = "RamGauge";
        private const string ram = "Ram"; // value
        private const string cores = "Cores"; // value
        private IHttpContextAccessor _httpContextAccessor;


        [HttpPost(ramUsage)]
        public async Task<IActionResult> RamUsage([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collector.GetData(ramUsage, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }
        [HttpPost(networkRecBytes)]
        public async Task<IActionResult> NetworkRecive([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collector.GetData(networkRecBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }
        [HttpPost(networkTransmitBytes)]
        public async Task<IActionResult> NetworkTransmit([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collector.GetData(networkTransmitBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }
        [HttpPost(network)]
        public async Task<IActionResult> NetworkBytes([FromBody] QueriesInfo queriesInfo)
        {
            Metric metricNetworkTransmit = await _collector.GetData(networkTransmitBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]);
            Metric metricNetworkRecive = await _collector.GetData(networkRecBytes, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]);
            return Ok(new Metric[] { metricNetworkTransmit, metricNetworkRecive });
        }
        [HttpPost(cpuUsage)]
        public async Task<IActionResult> CpuUsage([FromBody] QueriesInfo queriesInfo)
        {
            return Ok(await _collector.GetData(cpuUsage, queriesInfo.from, queriesInfo.to, Request.Headers[ServerUtils.MachineId]));
        }
        [HttpGet(cpuGauge)]
        public async Task<IActionResult> CpuGauge()
        {
            return Ok(new
            {
                result = (await _collector.GetData(cpuUsage, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow, Request.Headers[ServerUtils.MachineId])).DataPoints.Average(datapoint => datapoint.Value)
            ,
                type = "%"
            });
        }
        [HttpGet(ramGauge)]
        public async Task<IActionResult> RamGauge()
        {
            return Ok(new
            {
                result = ((await _collector.GetData(ramUsage, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, Request.Headers[ServerUtils.MachineId])).DataPoints.First().Value
                / (await _collector.GetData(ram, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, Request.Headers[ServerUtils.MachineId])).DataPoints.First().Value) * 100,
                type = "%"
            });
        }
        [HttpGet(ram)]
        public async Task<IActionResult> Ram()
        {
            return Ok(new { result = (await _collector.GetData(ram, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, Request.Headers[ServerUtils.MachineId])).DataPoints.First().Value, type = ((eNodeExporterData)Enum.Parse(typeof(eNodeExporterData), ram)).GetTypeValue() });
        }
        [HttpGet(cores)]
        public async Task<IActionResult> Cores()
        {
            return Ok(new { result = (await _collector.GetData(cores, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, Request.Headers[ServerUtils.MachineId])).DataPoints.First().Value, type=""});
        }
    }
}
