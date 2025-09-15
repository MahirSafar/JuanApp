using JuanApp.Application.Helper;
using JuanApp.Application.Models.ColorDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ColorController(IColorService colorService) : Controller
    {
        private const int PageSize = 4;
        public async Task<IActionResult> Index(int page = 1)
        {
            var colors = await colorService.GetAllAsync();
            var viewModels = colors.Select(s => new ColorViewModel
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();

            var paginatedList = new PaginatedList<ColorViewModel>(
                viewModels.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                viewModels.Count,
                page,
                PageSize
            );

            return View(paginatedList);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var color = await colorService.GetByIdAsync(id);
            if (color == null)
                return NotFound();
            var viewModel = new ColorViewModel
            {
                Id = color.Id,
                Name = color.Name,
            };
            return View(viewModel);
        }
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Color";
            return View(new ColorViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorViewModel viewModel)
        {
            ViewData["Title"] = "Create Color";
            if (!ModelState.IsValid)
                return View(viewModel);

            var color = new CreateColorDto
            {
                Name = viewModel.Name
            };
            await colorService.CreateAsync(color);
            TempData["SuccessMessage"] = "Color created successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Color";
            var color = await colorService.GetByIdAsync(id);
            if (color == null)
                return NotFound();

            var viewModel = new ColorViewModel
            {
                Id = color.Id,
                Name = color.Name,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ColorViewModel viewModel)
        {
            ViewData["Title"] = "Edit Color";
            if (id != viewModel.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(viewModel);
            var service = await colorService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            service.Name = viewModel.Name;
            
            await colorService.UpdateAsync(service);
            TempData["SuccessMessage"] = "Color updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var color = await colorService.GetByIdAsync(id);
            if (color == null)
                return NotFound();
            await colorService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Color deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
