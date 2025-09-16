using JuanApp.Application.Models.OrderDtos;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CheckoutDto checkoutDto, string userId, string sessionId);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<OrderDto?> GetOrderByNumberAsync(string orderNumber);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}