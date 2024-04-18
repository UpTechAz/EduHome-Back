using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Areas.Admin.ViewModels;
using WebApplication2.Constants;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or Password was incorrect");
                return View(model);
            }
            bool isSuperAdmin = await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin.ToString());
            if (!isSuperAdmin)
            {
                ModelState.AddModelError(string.Empty, "Username or password was incorrect");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username or Password was incorrect");
                return View(model);
            }
            return RedirectToAction("index", "dashboard");
        }
        #endregion
        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home", new { Areas = "" });
        }
        #endregion
    }
}
