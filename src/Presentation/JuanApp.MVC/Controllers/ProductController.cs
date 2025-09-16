using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using JuanApp.Application.Models.ReviewDtos;

namespace JuanApp.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;
        private readonly IReviewService _reviewService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IColorService colorService,
            ISizeService sizeService,
            ITagService tagService,
            IReviewService reviewService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _colorService = colorService;
            _sizeService = sizeService;
            _tagService = tagService;
            _reviewService = reviewService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            // Get all data needed for the product detail page
            var colors = await _colorService.GetAllAsync();
            var sizes = await _sizeService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();
            var allProducts = await _productService.GetAllAsync();

            // Get related products based on tags
            var relatedProducts = allProducts
                .Where(p => p.Id != product.Id && 
                           p.SelectedTagIds.Any(tagId => product.SelectedTagIds.Contains(tagId)))
                .Take(4)
                .Select(p => new ProductItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Discount = p.Discount,
                    MainImageUrl = p.MainImageUrl,
                    CategoryName = p.CategoryName,
                    Stock = p.Stock
                }).ToList();

            // If no related products by tags, get products from same category
            if (!relatedProducts.Any())
            {
                relatedProducts = allProducts
                    .Where(p => p.Id != product.Id && p.CategoryId == product.CategoryId)
                    .Take(4)
                    .Select(p => new ProductItemViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Discount = p.Discount,
                        MainImageUrl = p.MainImageUrl,
                        CategoryName = p.CategoryName,
                        Stock = p.Stock
                    }).ToList();
            }

            // Get reviews and review statistics
            var reviews = await _reviewService.GetApprovedReviewsByProductIdAsync(id);
            var reviewStatistics = await _reviewService.GetReviewStatisticsAsync(id);
            
            // Check if current user has already reviewed this product
            bool hasUserReviewed = false;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    hasUserReviewed = await _reviewService.HasUserReviewedProductAsync(userId, id);
                }
            }

            var viewModel = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Discount = product.Discount,
                Description = product.Description,
                Stock = product.Stock,
                Gender = product.Gender,
                MainImageUrl = product.MainImageUrl,
                CategoryName = product.CategoryName,
                ProductImages = product.ProductImages.Select(pi => new ProductImageItemViewModel
                {
                    Id = pi.Id,
                    ImageUrl = pi.ImageUrl
                }).ToList(),
                AvailableColors = colors.Where(c => product.SelectedColorIds.Contains(c.Id))
                    .Select(c => new ColorItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        HexCode = "" // Color entity doesn't have HexCode, use empty string
                    }).ToList(),
                AvailableSizes = sizes.Where(s => product.SelectedSizeIds.Contains(s.Id))
                    .Select(s => new SizeItemViewModel
                    {
                        Id = s.Id,
                        Size = s.ShoeSize.ToString()
                    }).ToList(),
                Tags = tags.Where(t => product.SelectedTagIds.Contains(t.Id))
                    .Select(t => new TagItemViewModel
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList(),
                RelatedProducts = relatedProducts,
                Reviews = reviews.Select(r => new ProductReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rate = r.Rate,
                    UserName = r.UserName,
                    UserFullName = r.UserFullName,
                    Created = r.Created
                }).ToList(),
                ReviewStatistics = new ReviewStatisticsViewModel
                {
                    TotalReviews = reviewStatistics.TotalReviews,
                    AverageRating = reviewStatistics.AverageRating,
                    FiveStarCount = reviewStatistics.FiveStarCount,
                    FourStarCount = reviewStatistics.FourStarCount,
                    ThreeStarCount = reviewStatistics.ThreeStarCount,
                    TwoStarCount = reviewStatistics.TwoStarCount,
                    OneStarCount = reviewStatistics.OneStarCount
                },
                HasUserReviewed = hasUserReviewed
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(int ProductId, int Rate, string Content)
        {
            try
            {
                // Validate input
                if (ProductId <= 0)
                {
                    return Json(new { success = false, message = "Invalid product." });
                }

                if (Rate < 1 || Rate > 5)
                {
                    return Json(new { success = false, message = "Rating must be between 1 and 5 stars." });
                }

                if (string.IsNullOrWhiteSpace(Content) || Content.Trim().Length < 10)
                {
                    return Json(new { success = false, message = "Review content must be at least 10 characters long." });
                }

                if (Content.Trim().Length > 500)
                {
                    return Json(new { success = false, message = "Review content cannot exceed 500 characters." });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not found. Please log in again." });
                }

                var createReviewDto = new CreateReviewDto
                {
                    Content = Content.Trim(),
                    Rate = Rate,
                    ProductId = ProductId,
                    AppUserId = userId
                };

                var result = await _reviewService.CreateReviewAsync(createReviewDto);

                return Json(new { 
                    success = result.Success, 
                    message = result.Message 
                });
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                return Json(new { success = false, message = "An error occurred while submitting your review. Please try again." });
            }
        }
    }
}