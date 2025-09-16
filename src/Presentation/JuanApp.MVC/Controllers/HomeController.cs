using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IServiceService _serviceService;
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;

        public HomeController(
            ISliderService sliderService,
            IServiceService serviceService,
            IProductService productService,
            IBlogService blogService,
            ISettingService settingService,
            ICategoryService categoryService)
        {
            _sliderService = sliderService;
            _serviceService = serviceService;
            _productService = productService;
            _blogService = blogService;
            _settingService = settingService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all data for the home page
                var sliders = await _sliderService.GetAllAsync();
                var services = await _serviceService.GetAllAsync();
                var allProducts = await _productService.GetAllAsync();
                var blogs = await _blogService.GetAllAsync();
                var settings = await _settingService.GetAllAsync();

                // Create view model
                var viewModel = new HomeIndexViewModel
                {
                    // Hero sliders
                    Sliders = sliders.Where(s => s.IsActive)
                                   .OrderBy(s => s.Order)
                                   .Select(s => new SliderItemViewModel
                                   {
                                       Id = s.Id,
                                       Title = s.Title,
                                       Subtitle = "", // No subtitle in slider model, use empty string
                                       Description = s.Description ?? "",
                                       ImageUrl = s.ImageUrl,
                                       ButtonText = "SHOP NOW", // Default button text
                                       ButtonUrl = s.RedirectUrl ?? "/products" // Use redirect URL or default to products
                                   }).ToList(),

                    // Service features (Free shipping, Support, etc.)
                    Services = services.Take(3).Select(s => new ServiceItemViewModel
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        Icon = s.Icon
                    }).ToList(),

                    // Featured products (carousel)
                    FeaturedProducts = allProducts.Take(8).Select(p => new ProductItemViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Discount = p.Discount,
                        MainImageUrl = p.MainImageUrl,
                        CategoryName = p.CategoryName,
                        Stock = p.Stock
                    }).ToList(),

                    // New products
                    NewProducts = allProducts.OrderByDescending(p => p.Created)
                                           .Take(6)
                                           .Select(p => new ProductItemViewModel
                                           {
                                               Id = p.Id,
                                               Name = p.Name,
                                               Price = p.Price,
                                               Discount = p.Discount,
                                               MainImageUrl = p.MainImageUrl,
                                               CategoryName = p.CategoryName,
                                               Stock = p.Stock
                                           }).ToList(),

                    // Latest blog posts
                    LatestBlogs = blogs.OrderByDescending(b => b.Created)
                                     .Take(4)
                                     .Select(b => new BlogItemViewModel
                                     {
                                         Id = b.Id,
                                         Title = b.Title,
                                         ImageUrl = b.ImageUrl,
                                         Author = b.Author,
                                         Created = b.Created
                                     }).ToList(),

                    // Site settings
                    Settings = settings.ToDictionary(s => s.Key, s => s.Value)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception here if you have logging configured
                // For now, return empty view model to prevent crashes
                var emptyViewModel = new HomeIndexViewModel
                {
                    Sliders = new List<SliderItemViewModel>(),
                    Services = new List<ServiceItemViewModel>(),
                    FeaturedProducts = new List<ProductItemViewModel>(),
                    NewProducts = new List<ProductItemViewModel>(),
                    LatestBlogs = new List<BlogItemViewModel>(),
                    Settings = new Dictionary<string, string>()
                };

                return View(emptyViewModel);
            }
        }
    }
}
