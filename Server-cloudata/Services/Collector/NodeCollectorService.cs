using BuisnessLogic.Collector;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.NodeExporter;
using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Markup;
using BuisnessLogic.Extentions;

namespace Server_cloudata.Services.Collector
{
    public class NodeCollectorService : ICollectorService<Metric>
    {
        public NodeCollectorService(ICollector<eNodeExporterData> collector, IBuilder<List<DataPoint>> builder) 
        {
           _collector = collector;
            _builder = builder;
        }
        
        private ICollector<eNodeExporterData> _collector;
        private IBuilder<List<DataPoint>> _builder;

        public async Task<Metric> GetData(string dataType, DateTime from, DateTime to, string address, params string[] values)
        {
            string jsonToConvert = await _collector.Collect(dataType, from, to, address, values);// instance(dns), what to collect, from and to 
            List<DataPoint> dataPoints = await _builder.Build(jsonToConvert);
            return new Metric(dataPoints, from, to, dataType, ((eNodeExporterData)Enum.Parse(typeof(eNodeExporterData), dataType)).GetTypeValue(), "Node exporter");
        }
    }
}
