using Microsoft.AspNetCore.Mvc;

namespace Server_cloudata.Controllers
{
    public class NodePredictDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
