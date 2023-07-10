using Microsoft.AspNetCore.Mvc;
using Server_cloudata.DTO;

namespace Server_cloudata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("AddHost")]
        public IActionResult AddMachine([FromBody] NewMachineDTO newMachineDTO)
        {
            //update data base and check validation
            //update prometheus file with the new DNSaddress/ip 
            return Ok();
        }
    }
}
