using Microsoft.AspNetCore.Mvc;
using Server_cloudata.DTO;
using System;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : Controller
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginBody)    //query="ramusage" start end                  todo to handle clients
        {
            //get information from data base 
            //check paswword and send relevent response
            try
            {
                int x = 5;                
                //update sessionId
                return Ok("{\"name\" : \"guy\"}");
            }
            catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }
        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] SignUpDTO signUpBody)    //query="ramusage" start end                  todo to handle clients
        {
            //object for sign up
            //check validation
            //update database async
            //send to client ok
            try
            {

                int x = 5;
                return Ok("{\"name\" : \"guy\"}");
            }
            catch (Exception ex) // server's execptions and Buissnes logic
            {
                return Conflict(ex); // todo
            }
        }


    }
}
