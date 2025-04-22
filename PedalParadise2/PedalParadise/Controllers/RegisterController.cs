using Microsoft.AspNetCore.Mvc;
using PedalParadise.Controllers.Data;
using PedalParadise.Models;

namespace PedalParadise.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index() {
            return View();
        }

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
