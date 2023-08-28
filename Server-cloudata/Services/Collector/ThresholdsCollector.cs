using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Server_cloudata.Models;
using Server_cloudata.ServerDataManager;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace Server_cloudata.Services.Collector
{
    public class ThresholdsCollector
    {
        private readonly CustomersService _customersService;
        private readonly IHttpContextAccessor _contextAccessor;
        private INodeCollectorService<Metric> _collector;

        public ThresholdsCollector(IHttpContextAccessor httpContextAccessor, CustomersService customersService, INodeCollectorService<Metric> collector)
        {
            _customersService = customersService;
            _contextAccessor = httpContextAccessor;
            _collector = collector;
        }

        public async Task<MetricWithThreshold> AddThresholsToMetric(Metric metric, string machineAddress)
        {
            try
            {
                var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));
                var virtualMachine = customer.VMs.Find(machine => machine.Address == machineAddress);
                return await AddThresholsToMetric(metric, virtualMachine);
            }
            catch (Exception)
            {
                return new MetricWithThreshold(metric);
            }
        }
        public async Task<MetricWithThreshold> AddThresholsToMetric(Metric metric, VirtualMachine virtualMachine)
        {
            try
            {
                MetricWithThreshold metricWithThreshold = new MetricWithThreshold(metric);
                if (virtualMachine.Thresholds.ContainsKey(metric.DataType))
                {
                    if (metric.DataType == eNodeExporterData.RamUsage.ToString())
                    {
                        float totalRam = (await _collector.GetData(eNodeExporterData.Ram.ToString(), DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, virtualMachine.Address)).DataPoints[0].Value;
                        metricWithThreshold.Warning = (virtualMachine.Thresholds[metric.DataType].Warning * totalRam) / 100;
                        metricWithThreshold.Critical = (virtualMachine.Thresholds[metric.DataType].Critical * totalRam) / 100;
                    }
                    else
                    {
                        metricWithThreshold.Warning = virtualMachine.Thresholds[metric.DataType].Warning;
                        metricWithThreshold.Critical = virtualMachine.Thresholds[metric.DataType].Critical;
                    }

                }
                return metricWithThreshold;
            }
            catch (Exception)
            {
                return new MetricWithThreshold(metric);
            }
        }
    }
}


