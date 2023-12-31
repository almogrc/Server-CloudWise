﻿

using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector;
using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuisnessLogic.Extentions;
using System.Linq;

namespace Server_cloudata.Services.Collector
{
    public class ProcessCollectorService : IProcessExporterService<List<Metric>>
    {
        public ProcessCollectorService(ICollector<eProcessExporterData> collector, IBuilder<Groups> builder)
        {
            _collector = collector;
            _builder = builder;
        }

        private ICollector<eProcessExporterData> _collector;
        private IBuilder<Groups> _builder;
        public async Task<List<Metric>> GetData(string dataType, DateTime from, DateTime to, string address, params string[] values)
        {
            string jsonToConvert = await _collector.Collect(dataType, from, to, address, values);// instance(dns), what to collect, from and to 
            Groups Groups = await _builder.Build(jsonToConvert);
            var topTenGroups = Groups.GroupNameToGroupData.OrderByDescending(groupNameToDate => groupNameToDate.Value.Sum(datapoint => datapoint.Value)).Take(10).ToList();
            List<Metric> Metrics = new List<Metric>();
            topTenGroups.ForEach(groupNameToData => 
            Metrics.Add(new Metric(groupNameToData.Value, from, to, dataType, ((eProcessExporterData)Enum.Parse(typeof(eProcessExporterData), dataType)).GetTypeValue(), groupNameToData.Key)));
            return Metrics;
           // return new Metric(dataPoints, from, to, dataType, ((eNodeExporterData)Enum.Parse(typeof(eNodeExporterData), dataType)).GetTypeValue(), "Node exporter");
        }
    }
}
