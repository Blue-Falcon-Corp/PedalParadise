using Microsoft.AspNetCore.Mvc;
using PedalParadise.Data;
using PedalParadise.Models;

namespace PedalParadise.Controllers
{
    public class RegisterController : Controller
    {
        private readonly PedalParadiseContext _context;

        public RegisterController(PedalParadiseContext context)
        {
            _context = context;
        }

        [Route("/Register")]
        public ActionResult Index() {
            return View();
        }

        [Route("/RegisterAttempt")]
        [HttpPost]
        public IActionResult Register(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                return RedirectToAction("Success");
            }
            return View(client);
        }

        public IActionResult Success()
        {
            //this one should redirect to log in page
            return View();
        }
    }
}
