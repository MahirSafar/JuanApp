using JuanApp.Application.Helper;
using JuanApp.Application.Models.TagDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class TagController(ITagService tagService) : Controller
    {
        private const int PageSize = 4;
        
        public async Task<IActionResult> Index(int page = 1)
        {
            var tags = await tagService.GetAllAsync();
            var viewModels = tags.Select(t => new TagViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug
            }).ToList();

            var paginatedList = new PaginatedList<TagViewModel>(
                viewModels.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                viewModels.Count,
                page,
                PageSize
            );

            return View(paginatedList);
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var tag = await tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound();
                
            var viewModel = new TagViewModel
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug
            };
            return View(viewModel);
        }
        
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Tag";
            return View(new TagViewModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagViewModel viewModel)
        {
            ViewData["Title"] = "Create Tag";
            if (!ModelState.IsValid)
                return View(viewModel);

            var tag = new CreateTagDto
            {
                Name = viewModel.Name,
                Slug = viewModel.Slug
            };
            await tagService.CreateAsync(tag);
            TempData["SuccessMessage"] = "Tag created successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Tag";
            var tag = await tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound();

            var viewModel = new TagViewModel
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagViewModel viewModel)
        {
            ViewData["Title"] = "Edit Tag";
            if (id != viewModel.Id)
                return NotFound();
                
            if (!ModelState.IsValid)
                return View(viewModel);
                
            var service = await tagService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            service.Name = viewModel.Name;
            service.Slug = viewModel.Slug;
            
            await tagService.UpdateAsync(service);
            TempData["SuccessMessage"] = "Tag updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound();
                
            await tagService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Tag deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}