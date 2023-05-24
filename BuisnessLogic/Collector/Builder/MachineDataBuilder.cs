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
    internal class MachineDataBuilder : IBuilder<MachineData>
    {
        private MachineData _machineData;
        public Dictionary<NodeExporterData, string> DataToConvert { get; set; }
        public MachineDataBuilder()
        {
            _machineData = new MachineData();
        }
        public void Build()
        {
            LinkedList<KeyValuePair<DateTime, double>> usageValues;
            foreach (NodeExporterData nodeExporeterData in DataToConvert.Keys)
            {
                usageValues = ConvertJsonToData(DataToConvert[nodeExporeterData]);
                _machineData.Data[nodeExporeterData] = usageValues;
            }
        }

        private LinkedList<KeyValuePair<DateTime, double>> ConvertJsonToData(string jsonString)
        {
            var json = convertToJsonAndCheckValidation(jsonString);
            var values = json.First["values"];
            LinkedList<KeyValuePair<DateTime, double>> usageValues = convertUsage(values);
            return usageValues;

        }
        LinkedList<KeyValuePair<DateTime, double>> convertUsage(dynamic values)
        {
            //to check
            LinkedList<KeyValuePair<DateTime, double>> dateTimeToMemoryUsage = new LinkedList<KeyValuePair<DateTime, double>>();
            DateTime dateTime;
            double usageValue;
            foreach (var value in values)
            {
                dateTime = unixSecondsToDateTime((long)value.First.Value);
                if (!double.TryParse(value.Last.Value, out usageValue))
                {
                    throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("double",
                           value.Last.Value.ToString()));
                }
                dateTimeToMemoryUsage.AddFirst(new KeyValuePair<DateTime, double>(dateTime, usageValue));
            }

            return dateTimeToMemoryUsage;
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
        public MachineData GetResult()
        {
            return _machineData;
        }
    }
}
