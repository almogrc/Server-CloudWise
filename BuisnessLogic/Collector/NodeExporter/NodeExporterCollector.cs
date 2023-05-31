using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.Exceptions;
using BuisnessLogic.Extentions;
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
        public NodeExporterCollector() : base("9100")
        {
            _data = new Dictionary<NodeExporterData, string>();
        }

        private string BuildQuery(NodeExporterData nodeExporeterData)
        {
            return $"{nodeExporeterData.GetStringValue()}{{instance=\"{Instance}\"}}";
        }
        public void Collect()
        {
            foreach (NodeExporterData nodeExporeterData in Enum.GetValues(typeof(NodeExporterData)))
            {
                Uri url;
                url = _prometheusAPI.BuildUrlQueryRange(BuildQuery(nodeExporeterData), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
                nodeExporeterData.GetStringValue();
                //switch (nodeExporeterData)
                //{
                //    case NodeExporterData.AvailabeBytes:
                //        url = _prometheusAPI.BuildUrlQueryRange(AvailabeMamory(), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
                //        break;
                //    default:
                //        throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("NodeExporterData",
                //           nodeExporeterData.ToString()));
                //}
                sendRequestAndUpdateData(url.AbsoluteUri, nodeExporeterData);
            }
            _machineDataBuilder.DataToConvert = _data;
            //option 1:

            Builder.Build();

            //option 2:

            //try
            //{
            //    Builder.Build();

            //}catch(UnknownTypeException ex)
            //{
            //    throw ex;
            //}
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
