using JuanApp.Application.Helper;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Enums;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private const int PageSize = 10;

        public ReviewController(IReviewService reviewService, IProductService productService)
        {
            _reviewService = reviewService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int page = 1, ReviewStatus? status = null, int? productId = null, string? searchTerm = null)
        {
            // Load filters first
            await LoadFiltersAsync();
            
            // Get all reviews
            var allReviews = await _reviewService.GetAllReviewsAsync();
            
            // Apply filters
            var filteredReviews = allReviews.AsEnumerable();
            
            if (status.HasValue)
            {
                filteredReviews = filteredReviews.Where(r => r.Status == status.Value);
            }
            
            if (productId.HasValue)
            {
                filteredReviews = filteredReviews.Where(r => r.ProductId == productId.Value);
            }
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredReviews = filteredReviews.Where(r => 
                    r.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    r.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    r.UserFullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            
            // Convert to ViewModels
            var reviewViewModels = filteredReviews.Select(r => new ReviewViewModel
            {
                Id = r.Id,
                Content = r.Content,
                Rate = r.Rate,
                Status = r.Status,
                ProductId = r.ProductId,
                AppUserId = r.AppUserId,
                UserName = r.UserName,
                UserFullName = r.UserFullName,
                Created = r.Created,
                LastModified = r.LastModified
            }).OrderByDescending(r => r.Created).ToList();

            // Get product names for display
            var products = await _productService.GetAllAsync();
            var productLookup = products.ToDictionary(p => p.Id, p => p.Name);
            
            foreach (var review in reviewViewModels)
            {
                if (productLookup.TryGetValue(review.ProductId, out var productName))
                {
                    review.ProductName = productName;
                }
            }

            // Create paginated list
            var paginatedList = new PaginatedList<ReviewViewModel>(
                reviewViewModels.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                reviewViewModels.Count,
                page,
                PageSize
            );
            
            // Store current filter values
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentProductId = productId;
            ViewBag.CurrentSearchTerm = searchTerm;

            return View(paginatedList);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            var product = await _productService.GetByIdAsync(review.ProductId);
            
            var viewModel = new ReviewViewModel
            {
                Id = review.Id,
                Content = review.Content,
                Rate = review.Rate,
                Status = review.Status,
                ProductId = review.ProductId,
                ProductName = product?.Name ?? "Unknown Product",
                AppUserId = review.AppUserId,
                UserName = review.UserName,
                UserFullName = review.UserFullName,
                Created = review.Created,
                LastModified = review.LastModified
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _reviewService.ApproveReviewAsync(id);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Review approved successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to approve review. Review may not exist.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _reviewService.RejectReviewAsync(id);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Review rejected successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to reject review. Review may not exist.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Review deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete review. Review may not exist.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            // Load filters first
            await LoadFiltersAsync();
            
            var pendingReviews = await _reviewService.GetPendingReviewsAsync();
            
            // Get product names for display
            var products = await _productService.GetAllAsync();
            var productLookup = products.ToDictionary(p => p.Id, p => p.Name);
            
            var viewModels = pendingReviews.Select(r => new ReviewViewModel
            {
                Id = r.Id,
                Content = r.Content,
                Rate = r.Rate,
                Status = r.Status,
                ProductId = r.ProductId,
                ProductName = productLookup.TryGetValue(r.ProductId, out var productName) ? productName : "Unknown Product",
                AppUserId = r.AppUserId,
                UserName = r.UserName,
                UserFullName = r.UserFullName,
                Created = r.Created,
                LastModified = r.LastModified
            }).OrderByDescending(r => r.Created).ToList();

            ViewBag.Title = "Pending Reviews";
            return View("Index", new PaginatedList<ReviewViewModel>(viewModels, viewModels.Count, 1, viewModels.Count));
        }

        private async Task LoadFiltersAsync()
        {
            // Load products for filter
            var products = await _productService.GetAllAsync();
            ViewBag.Products = new SelectList(products.Select(p => new { Id = p.Id, Name = p.Name }), "Id", "Name");
            
            // Load review statuses
            ViewBag.Statuses = new SelectList(Enum.GetValues<ReviewStatus>()
                .Select(s => new { Value = (int)s, Text = s.ToString() }), "Value", "Text");
        }
    }
}