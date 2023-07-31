using Microsoft.AspNetCore.Mvc;

namespace Server_cloudata.Controllers
{
    [ApiController]
    [Route("api/machine/[controller]")]
    public class AlertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
