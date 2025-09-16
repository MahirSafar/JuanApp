using JuanApp.Application.Models.BasketDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JuanApp.MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IProductService _productService;

        public BasketController(IBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var sessionId = GetOrCreateSessionId();
            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

            var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
            
            var viewModel = new BasketViewModel
            {
                Items = basketItems.Select(item => new BasketItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color,
                    ImageUrl = item.ImageUrl,
                    Stock = item.Stock
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(AddToBasketViewModel model)
        {
            try
            {
                if (model.Quantity <= 0)
                {
                    return Json(new { success = false, message = "Invalid quantity" });
                }

                var sessionId = GetOrCreateSessionId();
                var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                // Verify product exists and has stock
                var product = await _productService.GetByIdAsync(model.ProductId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                // Check if adding this quantity would exceed available stock
                var existingItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
                var existingQuantity = existingItems
                    .Where(x => x.ProductId == model.ProductId && 
                               x.Size == model.Size && 
                               x.Color == model.Color)
                    .Sum(x => x.Quantity);

                var totalQuantity = existingQuantity + model.Quantity;
                if (product.Stock < totalQuantity)
                {
                    var availableQuantity = product.Stock - existingQuantity;
                    if (availableQuantity <= 0)
                    {
                        return Json(new { success = false, message = "This item is already in your basket with maximum available quantity" });
                    }
                    return Json(new { success = false, message = $"Only {availableQuantity} more items can be added to basket" });
                }

                var createBasketItemDto = new CreateBasketItemDto
                {
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    Size = model.Size,
                    Color = model.Color,
                    SessionId = sessionId,
                    UserId = userId
                };

                var basketItem = await _basketService.AddToBasketAsync(createBasketItemDto);
                var itemCount = await _basketService.GetBasketItemCountAsync(sessionId, userId);

                return Json(new { 
                    success = true, 
                    message = "Product added to basket successfully",
                    itemCount = itemCount,
                    basketItem = new
                    {
                        id = basketItem.Id,
                        productName = basketItem.ProductName,
                        quantity = basketItem.Quantity,
                        price = basketItem.Price,
                        total = basketItem.Total
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = "An error occurred while adding the item to basket" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(UpdateBasketItemViewModel model)
        {
            try
            {
                if (model.Quantity < 0)
                {
                    return Json(new { success = false, message = "Invalid quantity" });
                }

                // If quantity is 0, remove the item
                if (model.Quantity == 0)
                {
                    return await RemoveItem(model.Id);
                }

                var updateDto = new UpdateBasketItemDto
                {
                    Id = model.Id,
                    Quantity = model.Quantity
                };

                await _basketService.UpdateBasketItemAsync(updateDto);

                var sessionId = GetOrCreateSessionId();
                var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
                
                var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
                var total = basketItems.Sum(x => x.Total);
                var itemCount = basketItems.Sum(x => x.Quantity);

                return Json(new { 
                    success = true, 
                    total = total.ToString("C"),
                    itemCount = itemCount,
                    message = "Quantity updated successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = "An error occurred while updating the quantity" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id)
        {
            try
            {
                var removed = await _basketService.RemoveFromBasketAsync(id);
                
                if (removed)
                {
                    var sessionId = GetOrCreateSessionId();
                    var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
                    
                    var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
                    var total = basketItems.Sum(x => x.Total);
                    var itemCount = basketItems.Sum(x => x.Quantity);

                    return Json(new { 
                        success = true, 
                        message = "Item removed from basket",
                        total = total.ToString("C"),
                        itemCount = itemCount
                    });
                }

                return Json(new { success = false, message = "Item not found" });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = "An error occurred while removing the item" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearBasket()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                var cleared = await _basketService.ClearBasketAsync(sessionId, userId);
                
                if (cleared)
                {
                    return Json(new { success = true, message = "Basket cleared successfully" });
                }

                return Json(new { success = false, message = "Basket is already empty" });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = "An error occurred while clearing the basket" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketCount()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                var count = await _basketService.GetBasketItemCountAsync(sessionId, userId);
                return Json(new { count = count });
            }
            catch
            {
                return Json(new { count = 0 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketItems()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
                
                var items = basketItems.Select(item => new
                {
                    id = item.Id,
                    productId = item.ProductId,
                    productName = item.ProductName,
                    price = item.Price,
                    quantity = item.Quantity,
                    size = item.Size,
                    color = item.Color,
                    imageUrl = item.ImageUrl,
                    total = item.Total
                }).ToList();

                var total = basketItems.Sum(x => x.Total);
                var itemCount = basketItems.Sum(x => x.Quantity);

                return Json(new { 
                    success = true, 
                    items = items,
                    total = total,
                    itemCount = itemCount
                });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = "An error occurred while retrieving basket items" });
            }
        }

        // Method to handle transferring basket items when user logs in
        [HttpPost]
        public async Task<IActionResult> TransferBasketOnLogin()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var sessionId = GetOrCreateSessionId();
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    
                    await _basketService.TransferBasketToUserAsync(sessionId, userId);
                    
                    // Don't delete the session ID cookie immediately, let it expire naturally
                    // This ensures smooth user experience
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = "An error occurred during basket transfer" });
            }
        }

        private string GetOrCreateSessionId()
        {
            var sessionId = Request.Cookies["BasketSessionId"];
            
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                Response.Cookies.Append("BasketSessionId", sessionId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax
                });
            }

            return sessionId;
        }
    }
}