using BuisnessLogic.MachineInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Enums;
using Newtonsoft.Json;

namespace BuisnessLogic.Collector.Builder
{
    internal class GroupBuilder : IBuilder<Group>
    {
        public Dictionary<string, Group> Groups { get; private set; }
        public Dictionary<ProcessExporeterData, string> DataToConvert { get; set; }
        //public GroupBuilder(Dictionary<ProcessExporeterData, string> dataToConvert)
        //{
        //    Processes = new Dictionary<string, Group>();
        //    _dataToConvert = dataToConvert;
        //}
        public void Build()
        {
            Groups.Clear();
            foreach (ProcessExporeterData processExporeterData in Enum.GetValues(typeof(ProcessExporeterData)))
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
                        throw new Exception("can't send request"); // almog to handle exception
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
                throw new Exception("invalid format file");
            }
            json = json["data"]["result"];
            return json;
        }
        void createOrUpdateGroupCpuUsage(string name, string mode, dynamic values)
        {
            Group groupToUpdate;
            CPUMode memoryType = convertFromStringToCpuType(mode);
            LinkedList<KeyValuePair<DateTime, int>> dateTimeToCpuUsage;
            createGroupIfNotExist(name);
            groupToUpdate = Groups[name];
            dateTimeToCpuUsage = convertUsage(values);
            groupToUpdate.CpuUsage.Add(memoryType, dateTimeToCpuUsage);

        }
        void createOrUpdateGroupMemoryUsage(string name, string memType, dynamic values) 
        {
            Group groupToUpdate;
            MemoryType memoryType = convertFromStringToMemoryType(memType);
            LinkedList<KeyValuePair<DateTime, int>> dateTimeToMemoryUsage;
            createGroupIfNotExist(name);
            groupToUpdate = Groups[name];
            dateTimeToMemoryUsage = convertUsage(values);
            groupToUpdate.MemoryUsage.Add(memoryType, dateTimeToMemoryUsage);
        }

        private void createGroupIfNotExist(string name)
        {
            if (!Groups.ContainsKey(name))
            {
                Groups[name] = new Group(name);
            }
        }

        LinkedList<KeyValuePair<DateTime, int>> convertUsage(dynamic values)
        {
            //to check
            LinkedList<KeyValuePair<DateTime, int>> dateTimeToMemoryUsage = new LinkedList<KeyValuePair<DateTime, int>>(values.Count);
            DateTime dateTime;
            int usageValue;
            foreach(var value in values)
            {
                dateTime = unixSecondsToDateTime(value[0]);
                if(!int.TryParse(value[1], out usageValue))
                {
                    throw new Exception("invalid format");
                }
                dateTimeToMemoryUsage.AddFirst(new KeyValuePair<DateTime, int>(dateTime, usageValue));
            }

            return dateTimeToMemoryUsage;
        }
        private MemoryType convertFromStringToMemoryType(string memType)
        {
            MemoryType result;
            if (Enum.TryParse<MemoryType>(memType, true, out result))
            {  // ignore cases
                throw new Exception("invalid format file almog dont be NPC");
            }

            return result;
        }
        private CPUMode convertFromStringToCpuType(string mode)
        {
            CPUMode result;
            if (Enum.TryParse<CPUMode>(mode, true, out result))
            {  // ignore cases
                throw new Exception("invalid format file almog dont be NPC");
            }

            return result;
        }
        private DateTime unixSecondsToDateTime(long timestamp, bool local = false)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }

        public Dictionary<string, Group> GetResult()
        {
            return Groups;
        }
    }
}
