using Microsoft.AspNetCore.Mvc;
using PedalParadise.Models.ViewModels;
using PedalParadise.Services;

namespace PedalParadise.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("/SignIn")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/SubmitLogin")]
        [HttpPost]
        public IActionResult SubmitLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //check credentials
                var user = _userService.AuthenticateAsync(/*here goes crdentials*/ model.Email, model.Password);
                if (user != null)
                {
                    return RedirectToAction("Dashboard"); // Redirect upon successful login
                }
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model); // Reload the page if validation fails
            }
        }
    }


        public IActionResult Logout()
        {
            //HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
}
