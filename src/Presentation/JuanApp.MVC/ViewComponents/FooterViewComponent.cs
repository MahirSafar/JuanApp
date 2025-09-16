using JuanApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;

        public FooterViewComponent(ISettingService settingService, ICategoryService categoryService)
        {
            _settingService = settingService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var settings = await _settingService.GetAllAsync();
                var categories = await _categoryService.GetAllAsync();

                var footerData = new FooterViewModel
                {
                    Settings = settings.ToDictionary(s => s.Key, s => s.Value),
                    Categories = categories.Take(6).Select(c => new FooterCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                };

                return View(footerData);
            }
            catch
            {
                // Return minimal footer data on error
                return View(new FooterViewModel
                {
                    Settings = new Dictionary<string, string>(),
                    Categories = new List<FooterCategoryViewModel>()
                });
            }
        }
    }

    public class FooterViewModel
    {
        public Dictionary<string, string> Settings { get; set; } = new();
        public List<FooterCategoryViewModel> Categories { get; set; } = new();
    }

    public class FooterCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}