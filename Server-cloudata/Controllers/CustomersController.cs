using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server_cloudata.Models;
using Server_cloudata.Services;
using MongoDB.Driver;

namespace Server_cloudata.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        /*public IActionResult Index()
        {
            return View();
        }*/

        private readonly CustomersService _customersService;

        public CustomersController(CustomersService customersService) =>
            _customersService = customersService;

        [HttpGet]
        public async Task<List<Customer>> Get() =>
            await _customersService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            var customer = await _customersService.GetAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer newCustomer)
        {
            await _customersService.CreateAsync(newCustomer);

            return CreatedAtAction(nameof(Get), new { id = newCustomer.Id }, newCustomer);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Customer updateCustomer)
        {
            var customer = await _customersService.GetAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            updateCustomer.Id = customer.Id;

            await _customersService.UpdateAsync(id, updateCustomer);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _customersService.GetAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            await _customersService.RemoveAsync(id);

            return NoContent();
        }
    }
}
