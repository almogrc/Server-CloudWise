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

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : Controller
    {
        private CustomersService _customersService;
        private IHttpContextAccessor _contextAccessor;
        private AlertsService _alertsService;

        public AlertController(IHttpContextAccessor httpContextAccessor, CustomersService customersService, AlertsService alertsService)
        {
            _contextAccessor = httpContextAccessor;
            _customersService = customersService;
            _alertsService = alertsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        
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

            await _alertsService.UpdateOrCreateAlertManager(customer.Email, request.MachineName);

            return Ok();
        }
    }
}
