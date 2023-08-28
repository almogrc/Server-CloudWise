using BuisnessLogic.Collector;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Extentions;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Server_cloudata.DTO;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services;
using Server_cloudata.Services.Collector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata.Controllers
{
    [Route("api/machine/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        public AlertController(CustomersService customersService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _customersService = customersService;
        }
        private CustomersService _customersService;
        private IHttpContextAccessor _httpContextAccessor;


        [HttpGet("GetThreshold")]
        public async Task<IActionResult> GetThreshold()
        {
            var customer = await _customersService.GetAsyncByEmail(_httpContextAccessor.HttpContext.Session.GetString(_httpContextAccessor.HttpContext.Session.Id));
            return Ok(customer.VMs.Find((machine) => machine.Address == Request.Headers[ServerUtils.MachineId]).Thresholds);
        }
      
    }
}
