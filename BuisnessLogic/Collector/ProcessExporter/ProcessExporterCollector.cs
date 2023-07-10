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

        public ProcessExporterCollector() : base("9256")
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

        public string BuildQuery(eProcessExporterData processExporeterDataType, string parms="")
        {
            return $"{processExporeterDataType.GetStringValue()}{{instance=\"{Instance}\"{parms}}}";
        }

        public void Collect()
        {
            foreach (eProcessExporterData processExporeterData in Enum.GetValues(typeof(eProcessExporterData)))
            {
                Uri url = _prometheusAPI.BuildUrlQueryRangeWithRate(BuildQuery(processExporeterData),
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

        public void Collect(eProcessExporterData ProcessExporterData, DateTime start, params string[] values)
        {
            Uri url;
            switch (ProcessExporterData)
            {
                case eProcessExporterData.cpu:
                    //to delete
                    url = _prometheusAPI.BuildUrlQueryRange(BuildQuery(ProcessExporterData, $",groupname=\"{values[0]}\",memtype=\"{eMemoryType.proportionalResident}\""), start, DateTime.UtcNow);
                    break;
                case eProcessExporterData.proportionalMemoryResident:
                    url = _prometheusAPI.BuildUrlQueryRange(BuildQuery(ProcessExporterData, $",groupname=\"{values[0]}\",memtype=\"{eMemoryType.proportionalResident}\""), start, DateTime.UtcNow);
                    break;
                case eProcessExporterData.readBytes:
                    url = _prometheusAPI.BuildUrlQueryRangeWithRate(BuildQuery(ProcessExporterData, $",groupname=\"{values[0]}\""), start, DateTime.UtcNow);
                    break;
                default:
                    throw new Exception("unknow type");
            }
            sendRequestAndUpdateData(url.AbsoluteUri, ProcessExporterData);
            _groupBuilder.DataToConvert = _data;
            Builder.Build(ProcessExporterData);
        }
    }
}
