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
    public class ProcessExporterCollector : AbstractExporter, ICollector<eProcessExporterData>// composer
    {
        public ProcessExporterCollector() : base("9256")
        {
        }

        //public string CPUQuery(string groupName, eCPUMode cpuMode = eCPUMode.user)
        //{
        //    return $"namedprocess_namegroup_cpu_seconds_total{{groupname=\"{groupName}\",mode=\"{cpuMode}\",instance=\"{Instance}\"}}";
        //}
        //public string MemoryQuery(string groupName, eMemoryType memoryType = eMemoryType.Resident)
        //{
        //    return $"namedprocess_namegroup_memory_bytes{{groupname=\"{groupName}\",memtype=\"{memoryType.ToString().ToLower()}\",instance=\"{Instance}\"}}";
        //}

        public string AddInstanceAnParamsToUrl(eProcessExporterData processExporeterDataType, string parms="")
        {
            return $"{processExporeterDataType.GetTypeValue()}{{instance=\"{Instance}\"{parms}}}";
        }
        private async Task<string> BuildQuery(string query, DateTime from, DateTime to, string address, params string[] values)
        {
            Uri url;
            IP = address;
            string nameGroupParam = values.Length > 0 ? $",groupname=\"{values[0]}\"" : "";
            eProcessExporterData ProcessExporterData;
            if (!Enum.TryParse(query, out ProcessExporterData))
            {
                throw new Exception("can't parse");
            }
            
            switch (ProcessExporterData)
            {
                case eProcessExporterData.cpu:
                    //to delete
                    url = _prometheusAPI.BuildUrlQueryRange(AddInstanceAnParamsToUrl(ProcessExporterData, $"{nameGroupParam}\",memtype=\"{eMemoryType.proportionalResident}\""), from, to);
                    break;
                case eProcessExporterData.proportionalMemoryResident:
                    url = _prometheusAPI.BuildUrlQueryRange(AddInstanceAnParamsToUrl(ProcessExporterData, $"{nameGroupParam},memtype=\"{eMemoryType.proportionalResident}\""), from, to);
                    break;
                case eProcessExporterData.readBytes:
                    url = _prometheusAPI.BuildUrlQueryRangeWithRate(AddInstanceAnParamsToUrl(ProcessExporterData, $"{nameGroupParam}"), from, to);
                    break;
                default:
                    throw new Exception("unknow type");
            }
            return url.AbsoluteUri;
        }
        public async Task<string> Collect(string query, DateTime from, DateTime to, string address, params string[] values)
        {
            string url = await BuildQuery(query, from, to, address, values);;
            return await _client.GetAsync(url);
        }

        public Task<string> Collect(string query,  string address)
        {
            throw new NotImplementedException();
        }
    }
}
