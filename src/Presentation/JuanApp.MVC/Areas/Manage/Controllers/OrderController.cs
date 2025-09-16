using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string status = "")
        {
            try
            {
                var allOrders = await _orderService.GetAllOrdersAsync();
                
                // Filter by status if provided
                if (!string.IsNullOrEmpty(status) && Enum.TryParse<Domain.Models.OrderStatus>(status, out var orderStatus))
                {
                    allOrders = allOrders.Where(o => o.Status == orderStatus);
                }

                // Calculate pagination
                var totalOrders = allOrders.Count();
                var totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);
                var orders = allOrders
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new OrderListViewModel
                {
                    Orders = orders.Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        OrderNumber = o.OrderNumber,
                        CustomerName = o.CustomerName,
                        CustomerSurname = o.CustomerSurname,
                        Address = o.Address,
                        City = o.City,
                        TotalAmount = o.TotalAmount,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
                        {
                            Id = oi.Id,
                            ProductName = oi.ProductName,
                            Price = oi.Price,
                            Quantity = oi.Quantity,
                            Size = oi.Size,
                            Color = oi.Color,
                            Total = oi.Total
                        }).ToList()
                    }).ToList(),
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalOrders,
                    StatusFilter = status
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading orders.";
                return View(new OrderListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("Index");
                }

                var viewModel = new OrderViewModel
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    UserId = order.UserId,
                    CustomerName = order.CustomerName,
                    CustomerSurname = order.CustomerSurname,
                    Address = order.Address,
                    City = order.City,
                    TotalAmount = order.TotalAmount,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        Id = oi.Id,
                        ProductName = oi.ProductName,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        Size = oi.Size,
                        Color = oi.Color,
                        Total = oi.Total
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the order details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, Domain.Models.OrderStatus status)
        {
            try
            {
                var success = await _orderService.UpdateOrderStatusAsync(id, status);
                if (success)
                {
                    TempData["Success"] = "Order status updated successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to update order status.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the order status.";
            }

            return RedirectToAction("Detail", new { id });
        }
    }
}