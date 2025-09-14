using JuanApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Controllers
{
    public class SubscribeController(ISubscriptionService subscriptionService, IEmailService emailService) : Controller
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;
        private readonly IEmailService _emailService = emailService;

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Email is required.";
                return RedirectToAction("Index", "Home");
            }

            var subscribeResult = await _subscriptionService.SubscribeAsync(email);
            if (!subscribeResult.Success)
            {
                TempData["ErrorMessage"] = subscribeResult.Error;
                return RedirectToAction("Index", "Home");
            }

            var tokenResult = await _subscriptionService.GenerateSubscribeConfirmationTokenAsync(email);
            if (!tokenResult.Success)
            {
                TempData["ErrorMessage"] = tokenResult.Error;
                return RedirectToAction("Index", "Home");
            }

            var confirmationLink = Url.Action("Confirm", "Subscribe",
                new { email = email, token = tokenResult.Token },
                Request.Scheme);

            using var reader = new StreamReader("wwwroot/templates/confirmSubscribe.html");
            string html = await reader.ReadToEndAsync();
            html = html.Replace("{{confirmationLink}}", confirmationLink);
            html = html.Replace("{{email}}", email);

            await _emailService.SendEmailAsync(
                email,
                "Confirm Your Newsletter Subscription",
                html);

            TempData["SuccessMessage"] = "Please check your email to confirm your subscription.";


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string email, string token)
        {
            var result = await _subscriptionService.ConfirmSubscriptionAsync(email, token);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction("Index", "Home");
            }

            await _emailService.SendEmailAsync(
                email,
                "Welcome to Our Newsletter!",
                "Thank you for confirming your subscription to our newsletter.");


            TempData["SuccessMessage"] = "Your subscription has been confirmed!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Unsubscribe(string email, string token)
        {
            var result = await _subscriptionService.UnsubscribeAsync(email, token);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction("Index", "Home");
            }

            TempData["SuccessMessage"] = "You have been successfully unsubscribed.";
            return RedirectToAction("Index", "Home");
        }
    }
}