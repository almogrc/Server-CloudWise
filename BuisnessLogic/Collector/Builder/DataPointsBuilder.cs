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
    public class DataPointsBuilder : IBuilder<List<DataPoint>>
    {
        public DataPointsBuilder()
        {
        }
        public async Task<List<DataPoint>> Build(string dataToConvert)
        {
            return await ConvertJsonToData(dataToConvert);
        }
        private async Task<List<DataPoint>> ConvertJsonToData(string jsonString)
        {
            var json = convertToJsonAndCheckValidation(jsonString);
            var values = json.First["values"];
            return convertUsage(values);
        }
        private List<DataPoint> convertUsage(dynamic values)
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

            return dateTimeToUsage.ToList();
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
    }
}
