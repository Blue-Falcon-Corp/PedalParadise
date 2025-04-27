// Controllers/AccountController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalParadise.Models;
using PedalParadise.Services;
using PedalParadise.Data;
using PedalParadise.Models.ViewModels;

namespace PedalParadise.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly PedalParadiseContext _context;

        public AccountController(IUserService userService, PedalParadiseContext context)
        {
            _userService = userService;
            _context = context;
        }

        // GET: /Account/Register
        [Route("/Register")]
        public IActionResult Register()
        {
            return View("RegisterViewModel");
        }
        [Route("/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already in use");
                    return View(model);
                }

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    UserType = "Client",
                    Password = model.Password // Ensure password is set
                };

                // Register the user
                await _userService.RegisterUserAsync(user, model.Password);

                // Create the client
                var client = new Client
                {
                    ClientID = user.UserID,
                    Membership = "Standard"
                };

                // Save client using the service
                await _userService.CreateClientAsync(client);

                // Set session
                HttpContext.Session.SetInt32("UserId", user.UserID);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("UserType", user.UserType);

                return RedirectToAction("Profile", "Account");
            }

            return View(model);
        }

        // GET: /Account/Login
        [Route("/Login")]
        public IActionResult Login()
        {
            return View("LoginViewModel");
        }

        // POST: /Account/Login
        [Route("/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.AuthenticateAsync(model.Email, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                // Set session
                HttpContext.Session.SetInt32("UserId", user.UserID);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("UserType", user.UserType);

                // create cart dynamically
                if (user.UserType == "Client")
                {
                    var existingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.ClientID == user.UserID);

                    if (existingCart == null)
                    {
                        var newCart = new ShoppingCart
                        {
                            ClientID = user.UserID,
                            DateCreated = DateTime.Now,
                        };

                        _context.ShoppingCarts.Add(newCart);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction("Profile", "Account");
            }

            return View(model);
        }

        // GET: /Account/Logout
        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Profile
        [Route("/Account/Profile")]
        public async Task<IActionResult> Profile()
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

        // GET: /Account/Edit
        [Route("/Account/Edit")]
        public async Task<IActionResult> Edit()
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

            var model = new EditProfileViewModel
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address
            };

            return View(model);
        }

        // POST: /Account/Edit
        [Route("/Account/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId != model.UserID)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByIdAsync(model.UserID);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Phone = model.Phone;
                user.Address = model.Address;

                await _userService.UpdateUserAsync(user);

                // Update session
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

                return RedirectToAction(nameof(Profile));
            }

            return View(model);
        }
    }
}