using JuanApp.Application.Helper;
using JuanApp.Application.Models;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(ISliderService sliderService, IWebHostEnvironment webHostEnvironment)
        {
            _sliderService = sliderService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllAsync();
            var viewModels = sliders.Select(s => new SliderViewModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                RedirectUrl = s.RedirectUrl,
                Order = s.Order,
                IsActive = s.IsActive
            });

            return View(viewModels);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
                return NotFound();

            var viewModel = new SliderViewModel
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description,
                ImageUrl = slider.ImageUrl,
                RedirectUrl = slider.RedirectUrl,
                Order = slider.Order,
                IsActive = slider.IsActive
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Create Slider";
            return View(new SliderViewModel { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderViewModel viewModel)
        {
            ViewData["Title"] = "Create Slider";

            if (!ModelState.IsValid)
                return View(viewModel);

            if (viewModel.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View(viewModel);
            }

            var slider = new CreateSliderDto
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                RedirectUrl = viewModel.RedirectUrl,
                Order = viewModel.Order,
                IsActive = viewModel.IsActive
            };

            slider.ImageUrl = viewModel.ImageFile.SaveFile("sliders");
            await _sliderService.CreateAsync(slider);

            TempData["SuccessMessage"] = "Slider created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Slider";
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
                return NotFound();

            var viewModel = new SliderViewModel
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description,
                ImageUrl = slider.ImageUrl,
                RedirectUrl = slider.RedirectUrl,
                Order = slider.Order,
                IsActive = slider.IsActive
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderViewModel viewModel)
        {
            ViewData["Title"] = "Edit Slider";

            if (id != viewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(viewModel);

            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
                return NotFound();

            slider.Title = viewModel.Title;
            slider.Description = viewModel.Description;
            slider.RedirectUrl = viewModel.RedirectUrl;
            slider.Order = viewModel.Order;
            slider.IsActive = viewModel.IsActive;

            if (viewModel.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(slider.ImageUrl))
                {
                    FileManager.DeleteFile("sliders", slider.ImageUrl);
                }
                viewModel.ImageFile.SaveFile("sliders");
            }

            await _sliderService.UpdateAsync(slider);

            TempData["SuccessMessage"] = "Slider updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
                return NotFound();

            if (!string.IsNullOrEmpty(slider.ImageUrl))
            {
                FileManager.DeleteFile("sliders", slider.ImageUrl);
            }

            await _sliderService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Slider deleted successfully.";


            return RedirectToAction(nameof(Index));
        }
    }
}