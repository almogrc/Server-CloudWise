using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using MongoDB.Driver;
using Server_cloudata.DTO;
using Server_cloudata.Models;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly CustomersService _customersService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ConnectionController(IHttpContextAccessor httpContextAccessor, CustomersService customersService)
        {
            _customersService = customersService;
            _contextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<List<Customer>> Get() =>
            await _customersService.GetAsync();

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginBody)    //query="ramusage" start end                  todo to handle clients
        {
            try
            {
                var customer = await _customersService.GetAsyncByEmail(loginBody.Email);

                if (customer == null)
                {
                    return NotFound(ServerUtils.ResultJson("Email not found."));
                }

                if (customer.Password == loginBody.Password)
                {
                    Response.Cookies.Append(ServerDataManager.ServerUtils.SessionCookie, _contextAccessor.HttpContext.Session.Id);
                    _contextAccessor.HttpContext.Session.SetString(_contextAccessor.HttpContext.Session.Id, loginBody.Email);
                    return Ok(new { name = customer.Name });
                }
                else
                {
                    return Unauthorized(ServerUtils.ResultJson("Incorrect password."));
                }
            }
            catch (Exception ex)
            {
                // Handle server exceptions and business logic errors
                return Conflict(ServerUtils.ResultJson(ex.Message));
            }
        }


        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] CustomerDTO signUpBodyDTO)
        {
            try
            {
                var signUpBody = new Customer
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = signUpBodyDTO.Name,
                    Email = signUpBodyDTO.Email,
                    Password = signUpBodyDTO.Password,
                    VMs = new List<VirtualMachine>()
                };

                var filter = Builders<Customer>.Filter.Or(
                    Builders<Customer>.Filter.Eq("email", signUpBody.Email)
                );

                var existingUser = _customersService._customersCollection.Find(filter).FirstOrDefault();

                if (existingUser != null)
                {
                    return Conflict(ServerUtils.ResultJson("User already exists"));
                }

                await _customersService.CreateAsync(signUpBody);
                
                return Ok(ServerUtils.ResultJson("User signed up successfully"));
            }
            catch (Exception ex)
            {
                // Handle any server exceptions or business logic errors
                return Conflict(ServerUtils.ResultJson(ex.Message));
            }
        }
    }
}
