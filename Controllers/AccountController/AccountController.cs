using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Areas.Admin.ViewModels;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Controllers.AccountController
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            User user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Password or username is invalid");
                return View(model);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Password or username is invalid");
                return View(model);
            }
            return RedirectToAction("Index", "Home", new { Areas = "" });
        }
        #endregion

        #region Logout
        [HttpGet]

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            User newUser = new User
            {
                FullName = model.Fullname,
                Email = model.Email,
                UserName = model.Username,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                };
                return View(model);
            }
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}
