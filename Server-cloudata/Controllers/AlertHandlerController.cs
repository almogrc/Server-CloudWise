using BuisnessLogic.Collector.Enums;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.Models;
using Server_cloudata.Services;
using static Server_cloudata.DTO.ThresholdDTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Server_cloudata.Models.PrometheusAlerts;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertHandlerController : Controller
    {
        private CustomersService _customersService;
        private IHttpContextAccessor _contextAccessor;
        private AlertsService _alertsService;

        public AlertHandlerController(IHttpContextAccessor httpContextAccessor, CustomersService customersService, AlertsService alertsService)
        {
            _contextAccessor = httpContextAccessor;
            _customersService = customersService;
            _alertsService = alertsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("alertReciver")]
        public async Task<IActionResult> GetAlertsFromPrometheus(AlertManagerWebhookData alerts)
        {
            await HandleAlerts(alerts);
            return Ok();
        }

        private async Task HandleAlerts(AlertManagerWebhookData alerts)
        {
            Dictionary<string, Models.PrometheusAlerts.AlertPrometheus> machineDnsToAlert = MakeDictionaryMachineDnsToAlert(alerts);
            List<Customer> customers = await _customersService.GetAsync();
            foreach (var customer in customers)
            {
                bool neededToUpdateCustomer = false;
                foreach(var virtualMachine in customer.VMs)
                {
                    if (machineDnsToAlert.ContainsKey(virtualMachine.Address))
                    {
                        if(virtualMachine.Alerts == null)
                        {
                            virtualMachine.Alerts = new List<Alert>();
                        }
                        virtualMachine.Alerts.Add(new Alert(machineDnsToAlert[virtualMachine.Address], virtualMachine));
                        neededToUpdateCustomer = true;
                    }
                }
                if (neededToUpdateCustomer)
                {
                    await _customersService.UpdateVMAsync(customer);
                }
            }
        }

        private Dictionary<string, Models.PrometheusAlerts.AlertPrometheus> MakeDictionaryMachineDnsToAlert(AlertManagerWebhookData alerts)
        {
            Dictionary<string, Models.PrometheusAlerts.AlertPrometheus> machineDnsToAlert = new Dictionary<string, Models.PrometheusAlerts.AlertPrometheus>();
            int lastIndex;
            foreach (var alert in alerts.Alerts)
            {
                lastIndex = alert.Labels.Instance.LastIndexOf(':');
                if(lastIndex != -1)
                {
                    string dnsWithoutPort = alert.Labels.Instance.Substring(0, lastIndex);
                    machineDnsToAlert.Add(dnsWithoutPort, alert);
                }
               
            }
            return machineDnsToAlert;
        }

    }
}
