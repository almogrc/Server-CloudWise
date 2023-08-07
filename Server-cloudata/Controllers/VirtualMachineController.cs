using BuisnessLogic.Collector.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_cloudata.DTO;
using Server_cloudata.Models;
using Server_cloudata.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Server_cloudata.DTO.ThresholdDTO;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : ControllerBase
    {
        private CustomersService _customersService;
        private IHttpContextAccessor _contextAccessor;

        public VirtualMachineController(IHttpContextAccessor httpContextAccessor, CustomersService customersService) 
        {
            _contextAccessor = httpContextAccessor;
            _customersService = customersService;
        }

        [HttpPost("AddVM")]
        public async Task<IActionResult> AddMachine([FromBody] VirtualMachine newMachine)
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));

            if (customer == null)
            {
                return NotFound();
            }

            if (customer.VMs == null)
            {
                customer.VMs = new List<VirtualMachine>();
            }

            if (customer.VMs.Any(vm => vm.Name == newMachine.Name))
            {
                return BadRequest("A machine with the same Name already exists.");
            }

            //TODO: check machine connection status

            customer.VMs.Add(newMachine);

            await _customersService.AddVMAsync(customer);

            // Update prometheus file with the new DNS address/IP

            return Ok();
        }

        [HttpPost("RemoveVM")]
        public async Task<IActionResult> RemoveMachine([FromBody] VirtualMachine machineToRemove)
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));

            if (customer == null || customer.VMs == null)
            {
                return NotFound();
            }

            var machine = customer.VMs.FirstOrDefault(vm => vm.Name == machineToRemove.Name);
            if (machine == null)
            {
                return NotFound();
            }

            customer.VMs.Remove(machine);

            await _customersService.AddVMAsync(customer);

            return Ok();
        }

        [HttpGet("GetAllVMs")]
        public async Task<IActionResult> GetAllVMs()
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));

            if (customer == null || customer.VMs == null)
            {
                return NotFound();
            }
            return Ok(customer.VMs);
        }
    }
}
