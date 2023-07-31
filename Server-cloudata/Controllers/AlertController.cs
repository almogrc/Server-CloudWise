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

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : Controller
    {
        private CustomersService _customersService;
        private IHttpContextAccessor _contextAccessor;

        public AlertController(IHttpContextAccessor httpContextAccessor, CustomersService customersService)
        {
            _contextAccessor = httpContextAccessor;
            _customersService = customersService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*
        [HttpPost("AddThreshold")]
        public async Task<IActionResult> AddThresholdToVM([FromBody] ThresholdRequest request)
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));
            if (customer == null)
            {
                return NotFound();
            }

            var virtualMachine = customer.VMs.FirstOrDefault(vm => vm.Name == request.MachineName);
            if (virtualMachine == null)
            {
                return NotFound("Machine with the given name not found.");
            }

            if (virtualMachine.Thresholds == null)
            {
                virtualMachine.Thresholds = new Dictionary<eNodeExporterData, double>();
            }

            if (virtualMachine.Thresholds.ContainsKey(request.Key))
            {
                virtualMachine.Thresholds[request.Key] = request.Value;
            }
            else
            {
                virtualMachine.Thresholds.Add(request.Key, request.Value);
            }

            await _customersService.UpdateVMAsync(customer);

            var timer = new Timer(async _ =>
            {
                await CheckThresholdsAndSendAlerts(customer, virtualMachine);
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Ok();
        }

        private async Task CheckThresholdsAndSendAlerts(Customer customer, VirtualMachine virtualMachine)
        {
            int vmData;

            if (vmData == null)
            {

                //Option: maybe log the error or handle it in other way
                return;
            }

            foreach (var key in virtualMachine.Thresholds.Keys)
            {
                if (vmData.Value > virtualMachine.Thresholds[key])
                {
                    var alert = new Alert
                    {
                    };

                    if (customer.Alerts == null)
                    {
                        customer.Alerts = new List<Alert>();
                    }
                    customer.Alerts.Add(alert);

                    await _customersService.UpdateVMAsync(customer);

                    await SendAlertEmail(alert);
                }
            }
        }

        private async Task SendAlertEmail(Alert alert)
        {

        }
        */
    }
}
