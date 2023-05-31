using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Enums;
using Newtonsoft.Json;
using BuisnessLogic.Exceptions;

namespace BuisnessLogic.Collector.Builder
{
    internal class GroupBuilder : IBuilder<Groups>
    {
        public Groups Groups { get; private set; }
        public Dictionary<ProcessExporeterData, string> DataToConvert { get; set; }
        public GroupBuilder()
        {
            Groups = new Groups();
        }
        public void Build()
        {
            Groups.GroupNameToGroupData.Clear();
            foreach (ProcessExporeterData processExporeterData in DataToConvert.Keys)
            {
                switch (processExporeterData)
                {
                    case ProcessExporeterData.cpu:
                        convertJsonCpuDataToGroup(DataToConvert[processExporeterData]);
                        break;
                    case ProcessExporeterData.memory:
                        convertJsonMemoryDataToGroup(DataToConvert[processExporeterData]);
                        break;
                    default:
                        throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("ProcessExporeterData",
                            processExporeterData.ToString()));
                }
            }
        }

        private void convertJsonCpuDataToGroup(string jsonCpuUsage)
        {
            var json = convertToJsonAndCheckValidation(jsonCpuUsage);
            string name;
            string mode;
            foreach (var group in json)
            {
                name = group["metric"]["groupname"];
                mode = group["metric"]["mode"];
                var values = group["values"];
                createOrUpdateGroupCpuUsage(name, mode, values);
            }

        }

        private void convertJsonMemoryDataToGroup(string jsonMemoryData)
        {
            var json = convertToJsonAndCheckValidation(jsonMemoryData);
            string name;
            string memType;
            foreach (var group in json)
            {
                name = group["metric"]["groupname"];
                memType = group["metric"]["memtype"];
                var values = group["values"];
                createOrUpdateGroupMemoryUsage(name, memType, values);
            }
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
        void createOrUpdateGroupCpuUsage(string groupName, string mode, dynamic values)
        {
            Group groupToUpdate;
            CPUMode cpuType = convertFromStringToCpuType(mode);
            LinkedList<DataPoint> dateTimeToCpuUsage;
            createGroupIfNotExist(groupName);
            groupToUpdate = Groups.GroupNameToGroupData[groupName];
            dateTimeToCpuUsage = convertUsage(values);
            groupToUpdate.CpuUsage.Add(cpuType, dateTimeToCpuUsage);

        }
        void createOrUpdateGroupMemoryUsage(string name, string memType, dynamic values) 
        {
            Group groupToUpdate;
            MemoryType memoryType = convertFromStringToMemoryType(memType);
            LinkedList<DataPoint> dateTimeToMemoryUsage;
            createGroupIfNotExist(name);
            groupToUpdate = Groups.GroupNameToGroupData[name];
            dateTimeToMemoryUsage = convertUsage(values);
            groupToUpdate.MemoryUsage.Add(memoryType, dateTimeToMemoryUsage);
        }

        private void createGroupIfNotExist(string name)
        {
            if (!Groups.GroupNameToGroupData.ContainsKey(name))
            {
                Groups.GroupNameToGroupData[name] = new Group(name);
            }
        }

        LinkedList<DataPoint> convertUsage(dynamic values)
        {
            //to check
            LinkedList<DataPoint> dateTimeToMemoryUsage = new LinkedList<DataPoint>();
            DateTime dateTime;
            float usageValue;
            foreach(var value in values)
            {
                dateTime = unixSecondsToDateTime((long)value.First.Value);
                if(!float.TryParse(value.Last.Value, out usageValue))
                {
                    throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("double",
                           value.Last.Value.ToString()));
                }
               
                dateTimeToMemoryUsage.AddFirst(new DataPoint { Date = dateTime, Value = usageValue });
            }

            return dateTimeToMemoryUsage;
        }
        private MemoryType convertFromStringToMemoryType(string memType)
        {
            MemoryType result;
            if (!Enum.TryParse<MemoryType>(memType, true, out result))
            {  // ignore cases
                throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("MemoryType",
                            result.ToString()));
            }

            return result;
        }
        private CPUMode convertFromStringToCpuType(string mode)
        {
            CPUMode result;
            if (!Enum.TryParse<CPUMode>(mode, true, out result))
            {  // ignore cases
                throw new UnexpectedTypeException(UnexpectedTypeException.BuildMessage("CPUMode",
                            result.ToString()));
            }

            return result;
        }
        private DateTime unixSecondsToDateTime(long timestamp, bool local = false)
        {  
            var offset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }

        public Groups GetResult()
        {
            return Groups;
        }
    }
}
