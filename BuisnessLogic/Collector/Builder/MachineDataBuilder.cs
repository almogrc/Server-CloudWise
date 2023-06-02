using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Exceptions;
using BuisnessLogic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.Builder
{
    internal class MachineDataBuilder : IBuilder<NodeData, eNodeExporterData>
    {
        private NodeData _nodeData;
        public Dictionary<eNodeExporterData, string> DataToConvert { get; set; }
        public MachineDataBuilder()
        {
            _nodeData = new NodeData();
            DataToConvert = new Dictionary<eNodeExporterData, string>();
        }
        public void Build()
        {
            LinkedList<DataPoint> usageValues;
            foreach (eNodeExporterData nodeExporeterData in DataToConvert.Keys)
            {
                usageValues = ConvertJsonToData(DataToConvert[nodeExporeterData]);
                _nodeData.Data[nodeExporeterData] = usageValues;
            }
        }
        private LinkedList<DataPoint> ConvertJsonToData(string jsonString)
        {
            var json = convertToJsonAndCheckValidation(jsonString);
            var values = json.First["values"];
            LinkedList<DataPoint> usageValues = convertUsage(values);
            return usageValues;
        }
        LinkedList<DataPoint> convertUsage(dynamic values)
        {
            //to check
            LinkedList<DataPoint> dateTimeToUsage = new LinkedList<DataPoint>();
            DateTime dateTime;
            float usageValue;
            foreach (var value in values)
            {
                dateTime = unixSecondsToDateTime((long)value.First.Value);
                if (!float.TryParse(value.Last.Value, out usageValue))
                {
                    throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("double",
                           value.Last.Value.ToString()));
                }
                dateTimeToUsage.AddLast(new DataPoint { Date = dateTime, Value = usageValue });
            }

            return dateTimeToUsage;
        }
        private DateTime unixSecondsToDateTime(long timestamp, bool local = false)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }
        private dynamic convertToJsonAndCheckValidation(string jsonString)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(jsonString);
            if (json["status"] != "success")
            {
                throw new UnsuccessfulResponseException($"JSON status - {json["status"]}");
            }
            json = json["data"]["result"];
            return json;
        }
        public NodeData GetResult()
        {
            return _nodeData;
        }
        public void Build(eNodeExporterData eData)
        {
            LinkedList<DataPoint> usageValues;
            usageValues = ConvertJsonToData(DataToConvert[eData]);
            _nodeData.Data[eData] = usageValues;
        }
        public NodeData GetResult(eNodeExporterData eData)
        {
            NodeData specifiecData = new NodeData();
            specifiecData.Data.Add(eData, _nodeData.Data[eData]);
            return specifiecData; 
        }
    }
}
