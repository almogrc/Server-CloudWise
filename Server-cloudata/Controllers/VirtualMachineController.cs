using BuisnessLogic.Collector;
using BuisnessLogic.Collector.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Server_cloudata.DTO;
using Server_cloudata.Models;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : ControllerBase
    {
        private CustomersService _customersService;
        private IHttpContextAccessor _contextAccessor;
        private ICollector<eNodeExporterData> _nodeCollector;

        public VirtualMachineController(IHttpContextAccessor httpContextAccessor, CustomersService customersService, ICollector<eNodeExporterData> nodeCollector) 
        {
            _contextAccessor = httpContextAccessor;
            _customersService = customersService;
            _nodeCollector = nodeCollector;
        }

        [HttpPost("AddVM")]
        public async Task<IActionResult> AddMachine([FromBody] VirtualMachineDTO newMachineDTO)
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));

            if (customer.VMs == null)
            {
                customer.VMs = new List<VirtualMachine>();
            }

            if (customer.VMs.Any(vm => vm.Name == newMachineDTO.Name))
            {
                return Conflict(ServerUtils.ErrorJson("A machine with the same Name already exists."));
            }

            var newMachine = new VirtualMachine
            {
                Name = newMachineDTO.Name,
                Supplier = newMachineDTO.Supplier.ToString(),
                Address = newMachineDTO.DNSAddress,
                Thresholds = GetDefualtMachineThreshold(), // we need to get the defualt data
                Alerts = new List<Alert>()
            };

            //TODO: check machine connection status
            
            customer.VMs.Add(newMachine);

            await _customersService.AddVMAsync(customer);
            // Update prometheus file with the new DNS address/IP
            Response.StatusCode = 201;
            return Ok(ServerUtils.ResultJson("Machine added successfully!"));
        }

        [HttpPost("RemoveVM")]
        public async Task<IActionResult> RemoveMachine([FromBody] VirtualMachineDTO machineToRemove)
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));

            var machine = customer.VMs.FirstOrDefault(vm => vm.Name == machineToRemove.Name);
            if (machine == null)
            {
                return NotFound(ServerUtils.ErrorJson($"A machine {machineToRemove.Name} not exists."));
            }

            customer.VMs.Remove(machine);

            await _customersService.AddVMAsync(customer);

            return Ok(ServerUtils.ResultJson($"Machine {machine.Name} removed successfully!"));
        }

        [HttpGet("GetAllVMs")]
        public async Task<IActionResult> GetAllVMs()
        {
            var customer = await _customersService.GetAsyncByEmail(_contextAccessor.HttpContext.Session.GetString(_contextAccessor.HttpContext.Session.Id));

            if (customer == null || customer.VMs == null)
            {
                return NotFound();
            }
            List<VirtualMachineDTO> virtualMachines = await ServerUtils.CheckMachineStatus(customer.VMs, await _nodeCollector.Collect("Status", ""));
            return Ok(virtualMachines);
        }

        private Dictionary<eNodeExporterData, ThresholdDTO> GetDefualtMachineThreshold()
        {
            string filePath = "properties/NodeExporterAlertConfig.json";

            try
            {
                var result = new Dictionary<eNodeExporterData, ThresholdDTO>();
                string jsonContent = System.IO.File.ReadAllText(filePath);
                List<ThresholdDTO>  thresholdDTO = JsonConvert.DeserializeObject<List<ThresholdDTO>>(jsonContent);
                thresholdDTO.ForEach(threshold =>
                {
                    eNodeExporterData value;
                    if (!Enum.TryParse(threshold.Name, ignoreCase: true, out value))
                    {
                        throw new Exception("cant parse");
                    }
                    result.Add(value, threshold);
                });
                return result;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private Dictionary<eProcessExporterData, Threshold> GetDefualtProcessThreshold()
        {
            string filePath = "properties/ProcessExporterAlertConfig.json"; 

            try
            {
                var result = new Dictionary<eProcessExporterData, Threshold>();
                string jsonContent = System.IO.File.ReadAllText(filePath);
                List<ThresholdDTO> thresholdDTO = JsonConvert.DeserializeObject<List<ThresholdDTO>>(jsonContent);
                thresholdDTO.ForEach(threshold => {
                    eProcessExporterData value;
                    if (!Enum.TryParse(threshold.Name, ignoreCase: true, out value))
                    {
                        throw new Exception("cant parse");
                    }

                    result.Add(value, threshold);
                    
                    });
                return result;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
