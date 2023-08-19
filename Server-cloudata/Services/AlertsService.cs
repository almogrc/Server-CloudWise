using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.NodeExporter;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Http;
using Server_cloudata.Models;
using Server_cloudata.Services.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Server_cloudata.Services
{
    public class AlertsService // singleton
    {
        private CustomersService _customersService;
        private Dictionary<string, AlertManager> _userIdToAlertManager;
        private Timer _timer;

        public AlertsService(CustomersService customersService) 
        {
            _userIdToAlertManager = new Dictionary<string, AlertManager>();
            _customersService = customersService;      
        }
        public void StartAlertRefresher()
        {
            _timer = new Timer(Start, null, TimeSpan.FromSeconds(3), Timeout.InfiniteTimeSpan);
        }
        //every 3 minutes run on all db customer and start alert refresher if needed
        private void Start(object state)
        {
            List<Customer> customers = _customersService.GetAsync().Result;
            foreach (Customer customer in customers)
            {
                foreach(VirtualMachine vm in customer.VMs)
                {
                    UpdateOrCreateAlertManager(customer.Email, vm.Name);
                }
            }
            _timer.Change(TimeSpan.FromMinutes(3), Timeout.InfiniteTimeSpan);
        }
        public async Task UpdateOrCreateAlertManager(string customerEmail, string machineId)
        {
            if (!_userIdToAlertManager.ContainsKey(customerEmail))
            {
               AlertManager alertManager = new AlertManager(_customersService, customerEmail);
               await alertManager.UpdateOrCreateThresholdRefresher(machineId);
               _userIdToAlertManager.Add(customerEmail, alertManager);
            }
        }
    }
}
