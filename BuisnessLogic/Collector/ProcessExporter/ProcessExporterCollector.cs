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

        private readonly long MBInBytes = 1048576;
        private readonly string MsInSecond = "1000";
        private readonly string KbPerSecond = "8/60";
        private readonly string IgnoreUnrelevantProcess = "";//"/ignoring(mode) namedprocess_namegroup_num_procs > 0";
        public string AddInstanceAnParamsToUrl(eProcessExporterData processExporeterDataType, string parms="")
        {
            return $"{processExporeterDataType.GetQueryValue()}{{instance=\"{Instance}\"{parms}}}";
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
                case eProcessExporterData.CpuUser:
                    //to delete
                    url = _prometheusAPI.BuildUrlQueryRange($"({_prometheusAPI.Irate(AddInstanceAnParamsToUrl(ProcessExporterData,$"{nameGroupParam},mode=\"{eCPUMode.user}\""),"30s")}{"*"}{MsInSecond}){IgnoreUnrelevantProcess}", from, to);
                    break;
                case eProcessExporterData.CpuSystem:
                    //to delete
                    url = _prometheusAPI.BuildUrlQueryRange($"({_prometheusAPI.Irate(AddInstanceAnParamsToUrl(ProcessExporterData, $"{nameGroupParam},mode=\"{eCPUMode.system}\""),"30s")}{"*"}{MsInSecond}){IgnoreUnrelevantProcess}", from, to);
                    break;
                case eProcessExporterData.ResidentMemory:
                    url = _prometheusAPI.BuildUrlQueryRange($"({AddInstanceAnParamsToUrl(ProcessExporterData, $"{nameGroupParam},memtype=\"{eMemoryType.Resident.ToString().ToLower()}\"")}/{MBInBytes}){IgnoreUnrelevantProcess}", from, to);
                    break;
                case eProcessExporterData.ReadBytes:
                    url = _prometheusAPI.BuildUrlQueryRange($"({_prometheusAPI.Irate(AddInstanceAnParamsToUrl(ProcessExporterData, $"{nameGroupParam}"))}){IgnoreUnrelevantProcess}", from, to);
                    break;
                default:
                    throw new Exception("unknow type");
            }
            return url.AbsoluteUri;
        }

        public async Task<string> Collect(string query, DateTime from, DateTime to, string address, params string[] values)
        {
            string url = await BuildQuery(query, from, to, address, values);
            return await _client.GetAsync(url);
        }

        public Task<string> Collect(string query,  string address)
        {
            throw new NotImplementedException();
        }
    }
}
