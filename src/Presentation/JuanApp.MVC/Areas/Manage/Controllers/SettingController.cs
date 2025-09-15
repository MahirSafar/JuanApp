using JuanApp.Application.Helper;
using JuanApp.Application.Models.SettingDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class SettingController(ISettingService settingService) : Controller
    {
        private const int PageSize = 10; // More settings may exist, so larger page size
        
        public async Task<IActionResult> Index(int page = 1)
        {
            var settings = await settingService.GetAllAsync();
            var viewModels = settings.Select(s => new SettingViewModel
            {
                Key = s.Key,
                Value = s.Value
            }).ToList();

            var paginatedList = new PaginatedList<SettingViewModel>(
                viewModels.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                viewModels.Count,
                page,
                PageSize
            );

            return View(paginatedList);
        }
        
        public async Task<IActionResult> Detail(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();
                
            var setting = await settingService.GetByKeyAsync(key);
            if (setting == null)
                return NotFound();
                
            var viewModel = new SettingViewModel
            {
                Key = setting.Key,
                Value = setting.Value
            };
            return View(viewModel);
        }
        
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Setting";
            return View(new SettingViewModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingViewModel viewModel)
        {
            ViewData["Title"] = "Create Setting";
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check if key already exists
            var keyExists = await settingService.KeyExistsAsync(viewModel.Key);
            if (keyExists)
            {
                ModelState.AddModelError("Key", "A setting with this key already exists.");
                return View(viewModel);
            }

            var setting = new CreateSettingDto
            {
                Key = viewModel.Key,
                Value = viewModel.Value
            };
            await settingService.CreateAsync(setting);
            TempData["SuccessMessage"] = "Setting created successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(string key)
        {
            ViewData["Title"] = "Edit Setting";
            if (string.IsNullOrEmpty(key))
                return NotFound();
                
            var setting = await settingService.GetByKeyAsync(key);
            if (setting == null)
                return NotFound();

            var viewModel = new SettingViewModel
            {
                Key = setting.Key,
                Value = setting.Value
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string key, SettingViewModel viewModel)
        {
            ViewData["Title"] = "Edit Setting";
            if (string.IsNullOrEmpty(key) || key != viewModel.Key)
                return NotFound();
                
            if (!ModelState.IsValid)
                return View(viewModel);
                
            var existingSetting = await settingService.GetByKeyAsync(key);
            if (existingSetting == null)
                return NotFound();

            var setting = new SettingDto
            {
                Key = viewModel.Key,
                Value = viewModel.Value
            };
            
            await settingService.UpdateAsync(setting);
            TempData["SuccessMessage"] = "Setting updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();
                
            var setting = await settingService.GetByKeyAsync(key);
            if (setting == null)
                return NotFound();
                
            await settingService.DeleteAsync(key);
            TempData["SuccessMessage"] = "Setting deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}