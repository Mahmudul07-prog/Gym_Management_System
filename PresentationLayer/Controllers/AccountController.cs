using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AccountController(IAccountService _accountService,
                                   SignInManager<ApplicationUser> _signInManager) : Controller
    {
        #region Login Action
        public IActionResult Login()
        {
            var userIsLogged = User.Identity.IsAuthenticated;
            if (userIsLogged)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var User = _accountService.ValidateUser(loginVM);
            if (User == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(loginVM);
            }

            var result = _signInManager.PasswordSignInAsync(User, loginVM.Password, loginVM.RememberMe, false).Result;

            if (result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Account Not Allowed");
            if (result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Account Locked");
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(loginVM);
        }
        #endregion

        #region Logout Action
        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Access Denied Action
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
