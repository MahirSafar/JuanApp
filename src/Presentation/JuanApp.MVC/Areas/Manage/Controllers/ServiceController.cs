using JuanApp.Application.Helper;
using JuanApp.Application.Models.ServiceDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        private const int PageSize = 4;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var services = await _serviceService.GetAllAsync();
            var viewModels = services.Select(s => new ServiceViewModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Icon = s.Icon,
            }).ToList();

            var paginatedList = new PaginatedList<ServiceViewModel>(
                viewModels.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                viewModels.Count,
                page,
                PageSize
            );

            return View(paginatedList);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
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

            await _serviceService.CreateAsync(service);
            TempData["SuccessMessage"] = "Service created successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit Serice";
            var service = await _serviceService.GetByIdAsync(id);
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
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            service.Title = viewModel.Title;
            service.Description = viewModel.Description;
            service.Icon = viewModel.Icon;

            await _serviceService.UpdateAsync(service)
                ;
            TempData["SuccessMessage"] = "Service updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();
            await _serviceService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Service deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

