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
        private readonly IOrderService _orderService;

        public AccountController(IUserService userService, PedalParadiseContext context, IOrderService orderService)
        {
            _userService = userService;
            _context = context;
            _orderService = orderService;
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
                    Discriminator = "Client",
                    Password = model.Password // Ensure password is set
                };

                // Register the user
                await _userService.RegisterUserAsync(user, model.Password);

                // Create the client
                var client = new Client
                {
                    Membership = "Standard"
                };

                // Save client using the service
                await _userService.CreateClientAsync(client);

                // Set session
                HttpContext.Session.SetInt32("UserId", user.UserID);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("UserType", user.Discriminator);

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
                HttpContext.Session.SetString("UserType", user.Discriminator);

                // create cart dynamically
                if (user.Discriminator == "Client") //use discriminator instead of usertype
                {
                    var existingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserID == user.UserID);

                    if (existingCart == null)
                    {
                        var newCart = new ShoppingCart
                        {
                            UserID = user.UserID,
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
            var orders = await _orderService.GetOrdersByClientIdAsync(userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }
            var viewModel = new ProfileViewModel
            {
                User = user,
                Orders = (List<Order>)orders
            };
            return View(viewModel);
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
            
            var model = new ProfileViewModel
            {
                User = user, // Directly assign User object to enable access to inherited properties
                Orders = (List<Order>)await _orderService.GetOrdersByClientIdAsync(userId.Value)
            };


            return View(model);
        }

        [Route("/Account/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
           // Get all model properties with values(filter out null ones)
            var providedProperties = typeof(ProfileViewModel).GetProperties()
                .Where(p => p.GetValue(model) != null)
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

            if (!ModelState.IsValid) return View(model);

            // Retrieve the user based on UserID passed from the form
            var user = await _userService.GetUserByIdAsync(model.User.UserID);
            if (user == null) return NotFound();

            // Update common fields
            user.FirstName = model.User.FirstName;
            user.LastName = model.User.LastName;
            user.Phone = model.User.Phone;
            user.Address = model.User.Address;
            user.Email = model.User.Email;

            // Save changes
            await _userService.UpdateUserAsync(user);

            // Redirect back to the profile page after successful update
            return RedirectToAction("Profile");
        }
        //GET: all accounts
        [Route("/Accounts/List")]
        public async Task<IActionResult> List()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserByIdAsync(userId.Value);

            var profileModel = new ProfileViewModel { User = user };
            var gente = await _userService.GetAllUsersAsync();//all accounts
            
            var employees = gente.OfType<Employee>().ToList();//employees
            var clients = gente.OfType<Client>().ToList();//clients

            var viewModel = new AccountPageViewModel
            {
                Profile = profileModel,
                Clients = clients,
                Employees = employees
            };

            return View(viewModel);
        }
    }
}