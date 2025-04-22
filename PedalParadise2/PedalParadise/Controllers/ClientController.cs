using Microsoft.AspNetCore.Mvc;
using PedalParadise.Controllers.Data;
using PedalParadise.Models;


namespace PedalParadise.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Client> Clients = _context.Clients;
            return View(Clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client empl)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(empl);
                _context.SaveChanges();
                TempData["ResultOK"] = "Record Added Successfully";
                return RedirectToAction("Index");
            }
            return View(empl);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empldb = _context.Clients.Find(id);
            if (empldb == null)
            {
                return NotFound();
            }
            return View(empldb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client empl)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Update(empl);
                _context.SaveChanges();
                TempData["ResultOK"] = "Data Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(empl);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var dbempl = _context.Clients.Find(id);

            if (dbempl == null)
            {
                return NotFound();
            }
            return View(dbempl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteClient(int? id)
        {
            var deleterecord = _context.Clients.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Clients.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOK"] = "Data Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
