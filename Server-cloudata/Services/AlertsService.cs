using BuisnessLogic.Model;
using Microsoft.AspNetCore.Http;
using Server_cloudata.Models;
using Server_cloudata.Services.Collector;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace Server_cloudata.Services
{
    public class AlertsService // singleton
    {
        private CustomersService _customersService;
        private Dictionary<string, AlertManager> _userIdToAlertManager;

        public AlertsService(CustomersService customersService) 
        {
            _userIdToAlertManager = new Dictionary<string, AlertManager>();
            _customersService = customersService;
        }

        public async Task UpdateOrCreateAlertManager(string customerEmail, string machineId)
        {
            if (_userIdToAlertManager.ContainsKey(customerEmail))
            {
                await _customersService.GetAsyncByEmail(customerEmail);    
            }
            else
            {
                AlertManager alertManager = new AlertManager(_customersService, customerEmail);
                await alertManager.UpdateOrCreateThresholdRefresher(machineId);
                _userIdToAlertManager.Add(customerEmail, alertManager);
            }
        }

    }
}
