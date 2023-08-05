using BuisnessLogic.Collector;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.DTO;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services.Collector;
using System.Collections.Generic;
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
        private const string ram = "Ram"; // value

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
        [HttpPost(ram)]
        public async Task<IActionResult> Ram()
        {
            return Ok(await _collectorNodeExporter.Collect(ram, Request.Headers[ServerUtils.MachineId]));
        }
    }
}
