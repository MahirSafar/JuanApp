using JuanApp.Application.Helper;
using JuanApp.Application.Models.CategoryDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        private const int PageSize = 4;
        
        public async Task<IActionResult> Index(int page = 1)
        {
            var categories = await categoryService.GetAllAsync();
            var viewModels = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            var paginatedList = new PaginatedList<CategoryViewModel>(
                viewModels.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                viewModels.Count,
                page,
                PageSize
            );

            return View(paginatedList);
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
                
            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return View(viewModel);
        }
        
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Category";
            return View(new CategoryViewModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel viewModel)
        {
            ViewData["Title"] = "Create Category";
            if (!ModelState.IsValid)
                return View(viewModel);

            var category = new CreateCategoryDto
            {
                Name = viewModel.Name
            };
            await categoryService.CreateAsync(category);
            TempData["SuccessMessage"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Category";
            var category = await categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
        {
            ViewData["Title"] = "Edit Category";
            if (id != viewModel.Id)
                return NotFound();
                
            if (!ModelState.IsValid)
                return View(viewModel);
                
            var service = await categoryService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            service.Name = viewModel.Name;
            
            await categoryService.UpdateAsync(service);
            TempData["SuccessMessage"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
                
            await categoryService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}