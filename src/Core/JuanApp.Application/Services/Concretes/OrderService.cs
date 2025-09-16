using AutoMapper;
using JuanApp.Application.Models.OrderDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Application.Services.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IBasketService basketService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _basketService = basketService;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(CheckoutDto checkoutDto, string userId, string sessionId)
        {
            // Get basket items
            var basketItems = await _basketService.GetBasketItemsAsync(sessionId, userId);
            
            if (!basketItems.Any())
            {
                throw new InvalidOperationException("Basket is empty");
            }

            // Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId,
                CustomerName = checkoutDto.CustomerName,
                CustomerSurname = checkoutDto.CustomerSurname,
                Address = checkoutDto.Address,
                City = checkoutDto.City,
                TotalAmount = basketItems.Sum(x => x.Total),
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = basketItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveAsync();

            // Clear the basket after successful order creation
            await _basketService.ClearBasketAsync(sessionId, userId);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            return order != null ? _mapper.Map<OrderDto>(order) : null;
        }

        public async Task<OrderDto?> GetOrderByNumberAsync(string orderNumber)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
            return order != null ? _mapper.Map<OrderDto>(order) : null;
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return false;
                }

                order.Status = status;
                _orderRepository.Update(order);
                await _orderRepository.SaveAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateOrderNumber()
        {
            return "ORD-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
        }
    }
}