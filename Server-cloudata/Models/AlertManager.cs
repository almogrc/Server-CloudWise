using Server_cloudata.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Server_cloudata.Services.Collector;
using BuisnessLogic.Model;
using BuisnessLogic.Collector;
using BuisnessLogic.Collector.NodeExporter;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using System.Linq;
using BuisnessLogic.Collector.Enums.Atributes;
using BuisnessLogic.Extentions;

namespace Server_cloudata.Models
{
    public class AlertManager
    {

        private Dictionary<string, ThresholdRefresher> _machineIdToThresholdRefresher;
        private string _customerEmail;
        private CustomersService _customersService;

        public AlertManager(CustomersService customersService, string customerEmail)
        {
            _customerEmail = customerEmail;
            _customersService = customersService;
            _machineIdToThresholdRefresher = new Dictionary<string, ThresholdRefresher>();
        }

        public async Task UpdateOrCreateThresholdRefresher(string machineId)
        {
            Customer customer = await _customersService.GetAsyncByEmail(_customerEmail);

            if(!_machineIdToThresholdRefresher.ContainsKey(machineId)) 
            {
                _machineIdToThresholdRefresher.Add(machineId, new ThresholdRefresher(_customersService, customer.VMs.Find(vm => vm.Name == machineId)));
            }
        }


        public class ThresholdRefresher
        {
            public ThresholdRefresher(CustomersService customersService, VirtualMachine virtualMachine)
            {
                _customersService = customersService;
                _virtualMachine = virtualMachine;
                _nodeCollector = new NodeCollectorService(new NodeExporterCollector(), new DataPointsBuilder());
                _timer = new Timer(Start, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
            }
            private CustomersService _customersService;
            private INodeCollectorService<Metric> _nodeCollector;
            private VirtualMachine _virtualMachine;
            private Timer _timer;

            public void Start(object state)
            {
                List<eNodeExporterData> allEnumValues = new List<eNodeExporterData>((eNodeExporterData[])Enum.GetValues(typeof(eNodeExporterData)));
                List<eNodeExporterData> nodeExporterValues = allEnumValues.Where(e => e.HasAttribute<AlertAttribute>()).ToList();
                Task.Run(async () =>
                {
                    foreach (var node in nodeExporterValues)
                    {
                        Metric metric = await _nodeCollector.GetData(node.ToString(), DateTime.Now, DateTime.Now, _virtualMachine.Address);
                       // metric.DataPoints.ForEach(dataPoint => dataPoint.Value)
                    }
                });
                

            }   

            public void StopTimer()
            {
                _timer.Dispose();
            }
        }   
    }

}
