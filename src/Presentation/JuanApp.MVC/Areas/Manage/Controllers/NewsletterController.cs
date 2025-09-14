using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class NewsletterController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public NewsletterController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public IActionResult Send()
        {
            return View(new NewsletterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(NewsletterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _subscriptionService.SendNewsletterAsync(model.Subject, model.Content);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error ?? "Failed to send newsletter.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Newsletter sent successfully!";
            return RedirectToAction("Send");
        }
    }
}