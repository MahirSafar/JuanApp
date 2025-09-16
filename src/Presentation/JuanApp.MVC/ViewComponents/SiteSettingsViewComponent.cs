using JuanApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.ViewComponents
{
    public class SiteSettingsViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public SiteSettingsViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var settings = await _settingService.GetAllAsync();
                var settingsDictionary = settings.ToDictionary(s => s.Key, s => s.Value);
                
                return View(settingsDictionary);
            }
            catch
            {
                // Return empty dictionary if there's an error
                // The layout should handle missing settings gracefully
                return View(new Dictionary<string, string>());
            }
        }
    }
}