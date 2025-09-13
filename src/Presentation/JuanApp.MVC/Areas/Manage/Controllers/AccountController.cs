using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController(IAccountService accountService) : Controller
    {
        private readonly IAccountService _accountService = accountService;

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = await _accountService.LoginAdminAsync(model.Username, model.Password, model.RememberMe);
            
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Invalid login attempt");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
