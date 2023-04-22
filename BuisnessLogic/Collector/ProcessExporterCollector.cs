using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.MachineInfo;

namespace BuisnessLogic.Collector
{
    internal class ProcessExporterCollector : ICollector<Group> // composer
    {
        private RequestClient _client = new RequestClient();
        private string Instance => $"{_ip}:{_port}";
        private string _ip = "localhost";
        private string _port = "9256";
        private PrometheusAPI _prometheusAPI = new PrometheusAPI();
        private Dictionary<ProcessExporeterData, string> _data { get; } = new Dictionary<ProcessExporeterData, string>();
        
        private GroupBuilder _groupBuilder = new GroupBuilder();
        public IBuilder<Group> Builder => _groupBuilder;

        public ProcessExporterCollector(string ip = "localhost", string port = "9256")
        {
            _port = port;
            _ip = ip;

        }

        public string CPUQuery(string groupName, CPUMode cpuMode = CPUMode.user)
        {
            return $"namedprocess_namegroup_cpu_seconds_total{{groupname=\"{groupName}\",mode=\"{cpuMode}\",instance=\"{Instance}\"}}";
        }
        public string MemoryQuery(string groupName, MemoryType memoryType = MemoryType.Resident)
        {
            return $"namedprocess_namegroup_memory_bytes{{groupname=\"{groupName}\",memtype=\"{memoryType}\",instance=\"{Instance}\"}}";
        }
        public string CPUAllQuery(CPUMode cpuMode = CPUMode.user)
        {
            return $"namedprocess_namegroup_cpu_seconds_total{{mode=\"{cpuMode}\",instance=\"{Instance}\"}}";
        }
        public string MemoryAllQuery(MemoryType memoryType = MemoryType.Resident)
        {
            return $"namedprocess_namegroup_memory_bytes{{memtype=\"{memoryType}\",instance=\"{Instance}\"}}";
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
