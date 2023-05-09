using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.Model;

namespace BuisnessLogic.Collector.ProcessExporter
{
    internal class ProcessExporterCollector : AbstractExporter, ICollector<Groups> // composer
    {
        private Dictionary<ProcessExporeterData, string> _data { get; } = new Dictionary<ProcessExporeterData, string>();
        private GroupBuilder _groupBuilder = new GroupBuilder();
        public IBuilder<Groups> Builder => _groupBuilder;

        public ProcessExporterCollector():base("9256")
        {       
        }

        public string CPUQuery(string groupName, CPUMode cpuMode = CPUMode.user)
        {
            return $"namedprocess_namegroup_cpu_seconds_total{{groupname=\"{groupName}\",mode=\"{cpuMode}\",instance=\"{Instance}\"}}";
        }
        public string MemoryQuery(string groupName, MemoryType memoryType = MemoryType.Resident)
        {
            return $"namedprocess_namegroup_memory_bytes{{groupname=\"{groupName}\",memtype=\"{memoryType.ToString().ToLower()}\",instance=\"{Instance}\"}}";
        }
        public string CPUAllQuery()
        {
            return $"namedprocess_namegroup_cpu_seconds_total{{instance=\"{Instance}\"}}";
        }
        public string MemoryAllQuery()
        {
            return $"namedprocess_namegroup_memory_bytes{{instance=\"{Instance}\"}}";
        }

        public void Collect()
        {
            foreach (ProcessExporeterData processExporeterData in Enum.GetValues(typeof(ProcessExporeterData)))
            {
                Uri url;

                switch (processExporeterData)
                {
                    case ProcessExporeterData.cpu:
                        url = _prometheusAPI.BuildUrlQueryRangeWithRate(CPUAllQuery(), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
                        break;
                    case ProcessExporeterData.memory:
                        url = _prometheusAPI.BuildUrlQueryRange(MemoryAllQuery(), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
                        break;
                    default:
                        throw new Exception("can't send request"); // almog to handle exception
                }
                sendRequestAndUpdateData(url.AbsoluteUri, processExporeterData);
            }
            _groupBuilder.DataToConvert = _data;
            Builder.Build();
        }
        private void sendRequestAndUpdateData(string url, ProcessExporeterData processExporeterData)
        {
            //send request
            string result = _client.GetAsync(url).Result;
            //update map
            _data[processExporeterData] = result;

        }      
        public void Collect(string information)
        {
            throw new NotImplementedException();
        }
    }
}
