using JuanApp.Application.Models.ServiceDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ServiceController(IServiceService serviceService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var services = await serviceService.GetAllAsync();
            var viewModels = services.Select(s => new ServiceViewModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Icon = s.Icon,
            });
            return View(viewModels);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var service = await serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();
            var viewModel = new ServiceViewModel
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                Icon = service.Icon,
            };
            return View(viewModel);
        }
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Service";
            return View(new ServiceViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel viewModel)
        {
            ViewData["Title"] = "Create Service";
            if (!ModelState.IsValid)
                return View(viewModel);

            var service = new CreateServiceDto
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Icon = viewModel.Icon,
            };

            await serviceService.CreateAsync(service);
            TempData["SuccessMessage"] = "Service created successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Serice";
            var service = await serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            var viewModel = new ServiceViewModel
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                Icon = service.Icon,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel viewModel)
        {
            ViewData["Title"] = "Edit Service";
            if (id != viewModel.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(viewModel);
            var service = await serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            service.Title = viewModel.Title;
            service.Description = viewModel.Description;
            service.Icon = viewModel.Icon;

            await serviceService.UpdateAsync(service)
                ;
            TempData["SuccessMessage"] = "Service updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();
            await serviceService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Service deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

