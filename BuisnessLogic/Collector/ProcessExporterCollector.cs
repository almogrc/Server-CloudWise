using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
namespace BuisnessLogic.Collector
{
    internal class ProcessExporterCollector : ICollector
    {
        private RequestClient _client = new RequestClient();
        private string Instance => $"{_ip}:{_port}";
        private string _ip = "localhost";
        private string _port = "9256";
        private PrometheusAPI _prometheusAPI = new PrometheusAPI();

        public Dictionary<ProcessExporeterData, string> Data { get; } = new Dictionary<ProcessExporeterData, string>();

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
                        url = _prometheusAPI.BuildUrlQueryRange(CPUAllQuery(), DateTime.Today.AddDays(-1), DateTime.Now);
                        break;
                    case ProcessExporeterData.memory:
                        url = _prometheusAPI.BuildUrlQueryRange(MemoryAllQuery(), DateTime.Today.AddDays(-1), DateTime.Now);
                        break;
                    default:
                        throw new Exception("can't send request"); // almog to handle exception
                }
                sendRequestAndUpdateData(url.AbsoluteUri, processExporeterData);
            }
        }
        private void sendRequestAndUpdateData(string url, ProcessExporeterData processExporeterData)
        {
            //send request
            string result = _client.GetAsync(url).Result;
            //update map
            Data[processExporeterData] = result;

        }
        public void Collect(string information)
        {
            throw new NotImplementedException();
        }
    }
}
