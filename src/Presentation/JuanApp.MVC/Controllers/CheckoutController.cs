using JuanApp.Application.Models.OrderDtos;
using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JuanApp.MVC.Controllers
{
    [Authorize(Roles = "Member")]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;

        public CheckoutController(IOrderService orderService, IBasketService basketService)
        {
            _orderService = orderService;
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessionId = GetOrCreateSessionId();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get basket items
            var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
            
            if (!basketItems.Any())
            {
                TempData["Error"] = "Your basket is empty. Add some items before proceeding to checkout.";
                return RedirectToAction("Index", "Basket");
            }

            var viewModel = new CheckoutViewModel
            {
                BasketItems = basketItems.Select(item => new BasketItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color,
                    ImageUrl = item.ImageUrl
                }).ToList(),
                TotalAmount = basketItems.Sum(x => x.Total)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload basket items for display
                var sessionId = GetOrCreateSessionId();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
                
                model.BasketItems = basketItems.Select(item => new BasketItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color,
                    ImageUrl = item.ImageUrl
                }).ToList();
                model.TotalAmount = basketItems.Sum(x => x.Total);

                return View(model);
            }

            try
            {
                var sessionId = GetOrCreateSessionId();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var checkoutDto = new CheckoutDto
                {
                    CustomerName = model.CustomerName,
                    CustomerSurname = model.CustomerSurname,
                    Address = model.Address,
                    City = model.City
                };

                var order = await _orderService.CreateOrderAsync(checkoutDto, userId, sessionId);

                TempData["Success"] = "Your order has been placed successfully!";
                return RedirectToAction("Success", new { orderNumber = order.OrderNumber });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Basket");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing your order. Please try again.";
                
                // Reload basket items for display
                var sessionId = GetOrCreateSessionId();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
                
                model.BasketItems = basketItems.Select(item => new BasketItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color,
                    ImageUrl = item.ImageUrl
                }).ToList();
                model.TotalAmount = basketItems.Sum(x => x.Total);

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Success(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return RedirectToAction("Index", "Home");
            }

            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            
            if (order == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("Index", "Home");
            }

            // Verify that this order belongs to the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.UserId != userId)
            {
                TempData["Error"] = "Access denied.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new CheckoutSuccessViewModel
            {
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                CustomerSurname = order.CustomerSurname,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(item => new OrderItemViewModel
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color,
                    Total = item.Total
                }).ToList()
            };

            return View(viewModel);
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