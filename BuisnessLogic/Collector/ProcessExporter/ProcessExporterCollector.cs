using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.Exceptions;
using BuisnessLogic.Extentions;
using BuisnessLogic.Model;

namespace BuisnessLogic.Collector.ProcessExporter
{
    internal class ProcessExporterCollector : AbstractExporter, ICollector<Groups, eProcessExporterData> // composer
    {
        private Dictionary<eProcessExporterData, string> _data { get; } = new Dictionary<eProcessExporterData, string>();
        private GroupBuilder _groupBuilder = new GroupBuilder();
        public IBuilder<Groups, eProcessExporterData> Builder => _groupBuilder;

        public ProcessExporterCollector():base("9256")
        {       
        }

        public string CPUQuery(string groupName, eCPUMode cpuMode = eCPUMode.user)
        {
            return $"namedprocess_namegroup_cpu_seconds_total{{groupname=\"{groupName}\",mode=\"{cpuMode}\",instance=\"{Instance}\"}}";
        }
        public string MemoryQuery(string groupName, eMemoryType memoryType = eMemoryType.Resident)
        {
            return $"namedprocess_namegroup_memory_bytes{{groupname=\"{groupName}\",memtype=\"{memoryType.ToString().ToLower()}\",instance=\"{Instance}\"}}";
        }
       
        public string AllDataQuery(eProcessExporterData processExporeterDataType)
        {
            return $"{processExporeterDataType.GetStringValue()}{{instance=\"{Instance}\"}}";
        }

        public void Collect()
        {
            foreach (eProcessExporterData processExporeterData in Enum.GetValues(typeof(eProcessExporterData)))
            {
                Uri url = _prometheusAPI.BuildUrlQueryRangeWithRate(AllDataQuery(processExporeterData),
                    DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);

                sendRequestAndUpdateData(url.AbsoluteUri, processExporeterData);
            }
            _groupBuilder.DataToConvert = _data;
            Builder.Build();
        }
        private void sendRequestAndUpdateData(string url, eProcessExporterData processExporeterData)
        {
            //send request
            string result = _client.GetAsync(url).Result;
            //update map
            _data[processExporeterData] = result;

        }

        public void Collect(eProcessExporterData query, DateTime start)
        {
            throw new NotImplementedException();
        }
    }
}
