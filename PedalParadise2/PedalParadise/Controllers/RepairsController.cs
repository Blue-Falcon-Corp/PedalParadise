using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models;
using PedalParadise.Services;
using System;
using System.Threading.Tasks;

namespace PedalParadise.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IRepairService _repairService;
        private readonly IUserService _userService;

        public RepairsController(IRepairService repairService, IUserService userService)
        {
            _repairService = repairService;
            _userService = userService;
        }

        // GET: /Repairs
        [Route("/Repairs")]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (userId == null)
            {
                // Show general repair info for non-logged in users
                return View("RepairService");
            }

            if (userType == "Employee")
            {
                // Show assigned repairs for employees
                var repairs = await _repairService.GetRepairsByEmployeeIdAsync(userId.Value);
                return View("EmployeeRepairs", repairs);
            }
            else
            {
                // Show user's repairs
                var repairs = await _repairService.GetRepairsByClientIdAsync(userId.Value);
                return View("Index", repairs);
            }
        }

        // GET: /Repairs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var repair = await _repairService.GetRepairByIdAsync(id);
            if (repair == null)
            {
                return NotFound();
            }

            // Access control
            if (userType != "Employee" && repair.UserID != userId)
            {
                return Forbid();
            }

            return View(repair);
        }

        // GET: /Repairs/Create
        [Route("Repairs/Create")]
        public async Task<IActionResult> CreateAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var users = await _userService.GetAllUsersAsync();
            ViewBag.Employees = users.OfType<Employee>().ToList();

            return View();
        }

        // POST: /Repairs/Create
        [Route("/Repairs/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairRequest repair)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // Get all model properties with values(filter out null ones)
            var providedProperties = typeof(RepairRequest).GetProperties()
                .Where(p => p.GetValue(repair) != null)
                .Select(p => p.Name)
                .ToList();

            // Remove validation for properties not in the request
            foreach (var property in ModelState.Keys.ToList())
            {
                if (!providedProperties.Contains(property))
                {
                    ModelState.Remove(property);
                }
            }
            
            if (ModelState.IsValid)
            {
                repair.UserID = userId.Value;
                repair.Status = "Pending";
                repair.SubmittedDate = DateTime.Now;

                await _repairService.CreateRepairRequestAsync(repair);
                TempData["SuccessMessage"] = "Your repair request has been submitted.";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var users = await _userService.GetAllUsersAsync();
                ViewBag.Employees = users.OfType<Employee>().ToList();
                return View(repair);
            }
        }

        // GET: /Repairs/UpdateStatus/5 (Employee only)
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            var repair = await _repairService.GetRepairByIdAsync(id);
            if (repair == null)
            {
                return NotFound();
            }

            ViewBag.StatusOptions = new[]
            {
                "Pending",
                "Diagnosis",
                "PartsOrdered",
                "InProgress",
                "Completed",
                "Closed"
            };

            return View(repair);
        }

        // POST: /Repairs/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("", "Status is required.");
                return RedirectToAction(nameof(UpdateStatus), new { id });
            }

            await _repairService.UpdateRepairStatusAsync(id, status);
            TempData["SuccessMessage"] = "Repair status updated successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: /Repairs/Assign/5 (Employee only)
        public async Task<IActionResult> Assign(int id)
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            var repair = await _repairService.GetRepairByIdAsync(id);
            if (repair == null)
            {
                return NotFound();
            }

            var mechanics = await _userService.GetEmployeesByRoleAsync("Mechanic");
            ViewBag.Mechanics = mechanics;

            return View(repair);
        }

        // POST: /Repairs/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, int employeeId)
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            await _repairService.AssignRepairToEmployeeAsync(id, employeeId);
            TempData["SuccessMessage"] = "Repair assigned successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}