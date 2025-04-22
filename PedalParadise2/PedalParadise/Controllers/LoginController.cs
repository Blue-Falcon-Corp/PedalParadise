using Microsoft.AspNetCore.Mvc;

namespace PedalParadise.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
