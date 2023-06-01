﻿using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.Prometheus;
using BuisnessLogic.Exceptions;
using BuisnessLogic.Extentions;
using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.NodeExporter
{
    internal class NodeExporterCollector : AbstractExporter, ICollector<NodeData>
    {
        private MachineDataBuilder _machineDataBuilder = new MachineDataBuilder();
        public IBuilder<NodeData> Builder => _machineDataBuilder;
        public Dictionary<eNodeExporterData, string> _data;
        public NodeExporterCollector() : base("9100")
        {
            _data = new Dictionary<eNodeExporterData, string>();
        }

        public string BuildQuery(eNodeExporterData nodeExporeterData)
        {
            return $"{nodeExporeterData.GetStringValue()}{{instance=\"{Instance}\"}}";
        }
        public void Collect()
        {
            foreach (eNodeExporterData nodeExporeterData in Enum.GetValues(typeof(eNodeExporterData)))
            {


                Uri url;
                url = _prometheusAPI.BuildUrlQueryRange(BuildQuery(nodeExporeterData), DateTime.UtcNow.AddMinutes(-30), DateTime.UtcNow);
                nodeExporeterData.GetStringValue();
                sendRequestAndUpdateData(url.AbsoluteUri, nodeExporeterData);
            }
            _machineDataBuilder.DataToConvert = _data;
            //option 1:

            Builder.Build();

            //option 2:

            //try
            //{
            //    Builder.Build();

            //}catch(UnknownTypeException ex)
            //{
            //    throw ex;
            //}
        }
        private void sendRequestAndUpdateData(string url, eNodeExporterData nodeExporeterData)
        {
            //send request
            string result = _client.GetAsync(url).Result;
            //update map
            _data[nodeExporeterData] = result;
        }
        public void Collect(string query, DateTime start)
        {
            eNodeExporterData eNodeExporter;
            if (!Enum.TryParse(query, true, out eNodeExporter)) { throw new Exception($"can't parse {query} to enum {"eNodeExporterData"}"); }

            Uri url;
            switch (eNodeExporter)
            {
                case eNodeExporterData.AvailabeBytes:
                case eNodeExporterData.MemTotalBytes:
                case eNodeExporterData.MemFreeBytes:
                case eNodeExporterData.MemCachedBytes:
                case eNodeExporterData.MemBuffersBytes:
                case eNodeExporterData.MemSRecliamableBytes:
                    url = _prometheusAPI.BuildUrlQueryRange(BuildQuery(eNodeExporter), start, DateTime.UtcNow);
                    break;
                case eNodeExporterData.RamUsage:
                    url = _prometheusAPI.BuildUrlQueryRange($"{BuildQuery(eNodeExporterData.MemTotalBytes)}-{BuildQuery(eNodeExporterData.MemFreeBytes)}-{BuildQuery(eNodeExporterData.MemCachedBytes)}-{BuildQuery(eNodeExporterData.MemBuffersBytes)}-{BuildQuery(eNodeExporterData.MemSRecliamableBytes)}", start, DateTime.UtcNow);
                    break;
                default:
                    throw new Exception("not valid type");
            }
            sendRequestAndUpdateData(url.AbsoluteUri, eNodeExporter);
        }
    }
}
