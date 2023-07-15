using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using MongoDB.Driver;
using Server_cloudata.DTO;
using Server_cloudata.Models;
using Server_cloudata.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : Controller
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
                    // Email does not exist in the collection
                    return NotFound("Email not found.");
                }

                if (customer.Password == loginBody.Password)
                {
                    Response.Cookies.Append(ServerDataManager.ServerDataManager.SessionCookie, _contextAccessor.HttpContext.Session.Id);
                    _contextAccessor.HttpContext.Session.SetString(_contextAccessor.HttpContext.Session.Id, loginBody.Email); // handle to save the session
                    return Ok(new { name = customer.Name }); // Return relevant response
                }
                else
                {
                    // Password does not match
                    return Unauthorized("Incorrect password.");
                }
            }
            catch (Exception ex)
            {
                // Handle server exceptions and business logic errors
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] Customer signUpBody)
        {
            try
            {
                // Check if the user already exists in the collection
                var filter = Builders<Customer>.Filter.Or(
                    Builders<Customer>.Filter.Eq("id", signUpBody.Id),
                    Builders<Customer>.Filter.Eq("email", signUpBody.Email)
                );
                var existingUser = _customersService._customersCollection.Find(filter).FirstOrDefault();

                if (existingUser != null)
                {
                    // User already exists, return appropriate error response
                    return Conflict("User already exists");
                }

                // User does not exist, add the new user to the collection

                await _customersService.CreateAsync(signUpBody);

                // Return success response to the client
                return Ok("User signed up successfully");
            }
            catch (Exception ex)
            {
                // Handle any server exceptions or business logic errors
                return Conflict(ex.Message);
            }
        }
    }
}
