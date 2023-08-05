using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.Exceptions;
using BuisnessLogic.Extentions;
using BuisnessLogic.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.NodeExporter
{
    public class NodeExporterCollector : AbstractExporter, ICollector<eNodeExporterData>
    {
       
        public NodeExporterCollector() : base("9100")
        {
        }
        private readonly long GBInBytes = 1073741824;
        private readonly string KbPerSecond = "8/60";
        private string AddInstanceToUrl(eNodeExporterData nodeExporeterData, string param="")
        { 
            return $"{nodeExporeterData.GetQueryValue()}{{instance=\"{Instance}\"{param}}}";
        }
        private async Task<string> BuildQuery(string query,DateTime from, DateTime to, string address, params string[] values)
        {
            Uri url;
            IP = address;
            eNodeExporterData eNodeExporter;
            if (!Enum.TryParse(query, out eNodeExporter))
            {
                throw new Exception("can't parse");
            }
            switch (eNodeExporter)
            {
                case eNodeExporterData.AvailabeBytes:
                case eNodeExporterData.MemTotalBytes:
                case eNodeExporterData.MemFreeBytes:
                case eNodeExporterData.MemCachedBytes:
                case eNodeExporterData.MemBuffersBytes:
                case eNodeExporterData.MemSRecliamableBytes:
                    url = _prometheusAPI.BuildUrlQueryRange(AddInstanceToUrl(eNodeExporter), from, to);
                    break;
                case eNodeExporterData.RamUsage:
                    url = _prometheusAPI.BuildUrlQueryRange
                        ($"{AddInstanceToUrl(eNodeExporterData.MemTotalBytes)}/{GBInBytes}-{AddInstanceToUrl(eNodeExporterData.MemFreeBytes)}/{GBInBytes}-{AddInstanceToUrl(eNodeExporterData.MemCachedBytes)}/{GBInBytes}-{AddInstanceToUrl(eNodeExporterData.MemBuffersBytes)}/{GBInBytes}-{AddInstanceToUrl(eNodeExporterData.MemSRecliamableBytes)}/{GBInBytes}", from, to);
                    break;
                case eNodeExporterData.NetworkRecBytes:
                case eNodeExporterData.NetworkTransmitBytes:
                    url = _prometheusAPI.BuildUrlQueryRangeWithIRate(AddInstanceToUrl(eNodeExporter, ",device=\"eth0\""), from, to, unit: KbPerSecond);
                    break;
                case eNodeExporterData.CPUUsage:
                    url = _prometheusAPI.BuildUrlQueryRange($"sum by(instance) ({_prometheusAPI.Irate(AddInstanceToUrl(eNodeExporter,", mode !=\"idle\""))}) / on(instance) group_left sum by (instance)({_prometheusAPI.Irate(AddInstanceToUrl(eNodeExporter))})*100", from, to);
                    break;
                case eNodeExporterData.Ram:
                    url = _prometheusAPI.BuildUrlQuery($"{AddInstanceToUrl(eNodeExporter)}/{GBInBytes}");
                    break;
                default:
                    throw new Exception("not valid type");
            }
            return url.AbsoluteUri;
        }
        public async Task<string> Collect(string query, DateTime from, DateTime to, string address, params string[] values)
        {
            string url = await BuildQuery(query, from, to, address, values);
            return await _client.GetAsync(url);
        }

        public async Task<string> Collect(string query, string address)
        {
            string url = await BuildQuery(query, DateTime.Now, DateTime.Now, address);
            return await _client.GetAsync(url);
        }

        //public async Task<string> Collect()
        //{
        //    foreach (eNodeExporterData nodeExporeterData in Enum.GetValues(typeof(eNodeExporterData)))
        //    {
        //        Uri url;
        //        url = _prometheusAPI.BuildUrlQueryRange(BuildQuery(nodeExporeterData), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
        //        nodeExporeterData.GetStringValue();
        //        GetData(url.AbsoluteUri, nodeExporeterData);
        //    }
        //    //  _machineDataBuilder.DataToConvert = _data;
        //
        //    _builder.Build(""); //to handle
        //}
    }
}
