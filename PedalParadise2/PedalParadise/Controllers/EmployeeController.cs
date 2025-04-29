using Microsoft.AspNetCore.Mvc;
using PedalParadise.Data;
using PedalParadise.Models;
using PedalParadise.Services;


namespace PedalParadise.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly PedalParadiseContext _context;
        private readonly IUserService _userService;
        public EmployeeController(IUserService userService, PedalParadiseContext context)
        {
            _userService = userService;
            _context = context;
        }
        [Route("/Employee/Dashboard")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee empl)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(empl);
                _context.SaveChanges();
                TempData["ResultOK"] = "Record Added Successfully";
                return RedirectToAction("Index");
            }
            return View(empl);
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var empldb = _context.Employees.Find(id);
            if (empldb == null)
            {
                return NotFound();
            }
            return View(empldb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee empl)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(empl);
                _context.SaveChanges();
                TempData["ResultOK"] = "Data Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(empl);
        }

        public IActionResult Delete(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var dbempl = _context.Employees.Find(id);

            if(dbempl == null)
            {
                return NotFound();
            }
            return View(dbempl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmp(int? id)
        {
            var deleterecord = _context.Employees.Find(id);
            if(deleterecord == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOK"] = "Data Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
