using Microsoft.AspNetCore.Mvc;

namespace Server_cloudata.Controllers
{
    public class AlertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
