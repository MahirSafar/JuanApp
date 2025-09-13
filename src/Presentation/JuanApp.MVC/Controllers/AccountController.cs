using JuanApp.Application.Services.Concretes;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JuanApp.MVC.Controllers
{
    public class AccountController(IAccountService accountService, IEmailService emailService) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVm userLoginVm, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View(userLoginVm);

            var result = await accountService.LoginUserAsync(userLoginVm.UsernameOrEmail, userLoginVm.Password, userLoginVm.RememberMe);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error);
                return View(userLoginVm);
            }
            HttpContext.Response.Cookies.Delete("basket");

            if (returnUrl is null)
                return RedirectToAction("Index", "Home");
            return Redirect(returnUrl);
        }

        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync();
            HttpContext.Response.Cookies.Delete("basket");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVm userRegisterVm)
        {
            if (!ModelState.IsValid)
                return View(userRegisterVm);

            var registerResult = await accountService.RegisterAsync(userRegisterVm.Username, userRegisterVm.Email, userRegisterVm.FullName, userRegisterVm.Password);

            if (!registerResult.Success)
            {
                ModelState.AddModelError("", registerResult.Error);
                return View(userRegisterVm);
            }

            var tokenResult = await accountService.GenerateEmailConfirmationTokenAsync(userRegisterVm.Username);
            if (tokenResult.Success)
            {
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email = userRegisterVm.Email, token = tokenResult.EmailConfirmationToken }, Request.Scheme);

                using StreamReader reader = new StreamReader("wwwroot/templates/emailConfirmTemplate.html");
                string html = await reader.ReadToEndAsync();
                html = html.Replace("{{confirmationLink}}", confirmationLink);
                html = html.Replace("{{name}}", userRegisterVm.Username);

                emailService.SendEmail(userRegisterVm.Email, "Email Confirmation", html);
            }

            TempData["SuccessMessage"] = "Registration successful! Please check your email to confirm your account.";

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation link.");
            }

            var result = await accountService.ConfirmEmailAsync(email, token);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Your email has been successfully confirmed. You can now log in.";
                return RedirectToAction(nameof(Login));
            }

            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVm forgotPasswordVm)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordVm);
            }

            var tokenResult = await accountService.GeneratePasswordResetTokenAsync(forgotPasswordVm.Email);

            if (!tokenResult.Success)
            {
                // To prevent email enumeration attacks, it's best not to reveal if the email exists.
                // We'll return a success message regardless of whether the email was found.
                TempData["SuccessMessage"] = "If an account with that email exists, a password reset link has been sent.";
                return RedirectToAction(nameof(ForgotPassword));
            }

            var confirmationLink = Url.Action("ResetPassword", "Account", new { email = forgotPasswordVm.Email, token = tokenResult.PasswordResetToken }, Request.Scheme);

            using StreamReader reader = new StreamReader("wwwroot/templates/forgotPasswordTemplate.html");
            string html = await reader.ReadToEndAsync();
            html = html.Replace("{{confirmationLink}}", confirmationLink);
            // Since the user might not have a username, we will use a more generic greeting or the email.
            html = html.Replace("{{name}}", forgotPasswordVm.Email);
            emailService.SendEmail(forgotPasswordVm.Email, "Reset Password", html);

            TempData["SuccessMessage"] = "If an account with that email exists, a password reset link has been sent.";
            return RedirectToAction(nameof(ForgotPassword));
        }

        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Invalid password reset link.");
                return View(new ResetPasswordVm());
            }

            var resetPasswordVm = new ResetPasswordVm
            {
                Email = email,
                Token = token
            };

            return View(resetPasswordVm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm resetPasswordVm)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVm);
            }

            var result = await accountService.ResetPasswordAsync(resetPasswordVm.Email, resetPasswordVm.Token, resetPasswordVm.NewPassword);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error);
                return View(resetPasswordVm);
            }

            TempData["SuccessMessage"] = "Your password has been reset successfully. Please log in with your new password.";
            return RedirectToAction(nameof(Login));
        }
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (result.Succeeded)
            {
                var emailClaim = result.Principal.FindFirst(ClaimTypes.Email);
                var nameClaim = result.Principal.FindFirst(ClaimTypes.Name);
                var googleIdClaim = result.Principal.FindFirst(ClaimTypes.NameIdentifier);

                if (emailClaim != null && nameClaim != null && googleIdClaim != null)
                {
                    // Call the new service method to handle the entire process
                    var loginResult = await accountService.HandleExternalLoginAsync(
                        email: emailClaim.Value,
                        fullName: nameClaim.Value,
                        externalId: googleIdClaim.Value);

                    if (loginResult.Success)
                    {
                        HttpContext.Response.Cookies.Delete("basket");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Login", new { error = "Google login failed." });
        }
    }
}
