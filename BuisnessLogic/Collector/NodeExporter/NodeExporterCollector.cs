using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.NodeExporter
{
    internal class NodeExporterCollector : AbstractExporter, ICollector<MachineData>
    {
        private MachineDataBuilder _machineDataBuilder = new MachineDataBuilder();
        public IBuilder<MachineData> Builder => _machineDataBuilder;
        public Dictionary<NodeExporterData, string> _data;
        public NodeExporterCollector():base("9100")
        {
            _data = new Dictionary<NodeExporterData, string>();
        }

        private string AvailabeMamory()
        {
            return $"node_memory_MemAvailable_bytes{{instance=\"{Instance}\"}}";
        }

        public void Collect()
        {
            foreach (NodeExporterData nodeExporeterData in Enum.GetValues(typeof(NodeExporterData)))
            {
                Uri url;

                switch (nodeExporeterData)
                {
                    case NodeExporterData.AvailabeBytes:
                        url = _prometheusAPI.BuildUrlQueryRange(AvailabeMamory(), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
                        break;
                    default:
                        throw new Exception("can't send request"); // almog to handle exception
                }
                sendRequestAndUpdateData(url.AbsoluteUri, nodeExporeterData);
            }
            _machineDataBuilder.DataToConvert = _data;
            Builder.Build();
        }

        private void sendRequestAndUpdateData(string url, NodeExporterData nodeExporeterData)
        {
            //send request
            string result = _client.GetAsync(url).Result;
            //update map
            _data[nodeExporeterData] = result;
        }

            public void Collect(string information)
        {
            throw new NotImplementedException();
        }
    }
}
