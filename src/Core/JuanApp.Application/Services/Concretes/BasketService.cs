using AutoMapper;
using JuanApp.Application.Models.BasketDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Application.Services.Concretes
{
    public class BasketService : IBasketService
    {
        private readonly IGenericRepository<BasketItem> _basketRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public BasketService(
            IGenericRepository<BasketItem> basketRepository,
            IGenericRepository<Product> productRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BasketItemDto>> GetBasketItemsAsync(string sessionId, string? userId = null)
        {
            var query = _basketRepository.GetWhere(x => 
                (x.SessionId == sessionId && x.UserId == null) || 
                (userId != null && x.UserId == userId))
                .Include(x => x.Product);
            
            var basketItems = await query.ToListAsync();
            return _mapper.Map<IEnumerable<BasketItemDto>>(basketItems);
        }

        public async Task<BasketItemDto?> GetBasketItemAsync(int id)
        {
            var basketItem = await _basketRepository.GetWhere(x => x.Id == id)
                                                   .Include(x => x.Product)
                                                   .FirstOrDefaultAsync();
            
            return basketItem != null ? _mapper.Map<BasketItemDto>(basketItem) : null;
        }

        public async Task<BasketItemDto> AddToBasketAsync(CreateBasketItemDto createBasketItemDto)
        {
            // Validate input
            if (createBasketItemDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            if (string.IsNullOrEmpty(createBasketItemDto.SessionId))
                throw new ArgumentException("Session ID is required");

            // Check if product exists
            var product = await _productRepository.GetByIdAsync(createBasketItemDto.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found");

            if (product.Stock < createBasketItemDto.Quantity)
                throw new InvalidOperationException("Insufficient stock");

            // Check if item already exists in basket with same attributes
            var existingItem = await _basketRepository.GetWhere(x => 
                x.ProductId == createBasketItemDto.ProductId &&
                ((x.SessionId == createBasketItemDto.SessionId && x.UserId == null) ||
                 (createBasketItemDto.UserId != null && x.UserId == createBasketItemDto.UserId)) &&
                x.Size == createBasketItemDto.Size &&
                x.Color == createBasketItemDto.Color)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                // Update quantity of existing item
                var newQuantity = existingItem.Quantity + createBasketItemDto.Quantity;
                if (product.Stock < newQuantity)
                    throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}, Requested: {newQuantity}");

                existingItem.Quantity = newQuantity;
                _basketRepository.Update(existingItem);
                await _basketRepository.SaveAsync();

                var existingItemWithProduct = await _basketRepository.GetWhere(x => x.Id == existingItem.Id)
                                                                     .Include(x => x.Product)
                                                                     .FirstAsync();
                return _mapper.Map<BasketItemDto>(existingItemWithProduct);
            }

            // Create new basket item
            var basketItem = _mapper.Map<BasketItem>(createBasketItemDto);
            basketItem.ProductName = product.Name;
            basketItem.Price = product.Discount > 0 ? product.Price - product.Discount : product.Price;
            basketItem.ImageUrl = product.MainImageUrl;

            var addedItem = await _basketRepository.AddAsync(basketItem);
            await _basketRepository.SaveAsync();

            var addedItemWithProduct = await _basketRepository.GetWhere(x => x.Id == addedItem.Id)
                                                             .Include(x => x.Product)
                                                             .FirstAsync();
            return _mapper.Map<BasketItemDto>(addedItemWithProduct);
        }

        public async Task<BasketItemDto> UpdateBasketItemAsync(UpdateBasketItemDto updateBasketItemDto)
        {
            if (updateBasketItemDto.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative");

            var basketItem = await _basketRepository.GetWhere(x => x.Id == updateBasketItemDto.Id)
                                                   .Include(x => x.Product)
                                                   .FirstOrDefaultAsync();

            if (basketItem == null)
                throw new ArgumentException("Basket item not found");

            if (basketItem.Product == null)
                throw new InvalidOperationException("Product information not found");

            if (updateBasketItemDto.Quantity == 0)
            {
                // Remove item if quantity is 0
                _basketRepository.Remove(basketItem);
                await _basketRepository.SaveAsync();
                return _mapper.Map<BasketItemDto>(basketItem);
            }

            if (basketItem.Product.Stock < updateBasketItemDto.Quantity)
                throw new InvalidOperationException($"Insufficient stock. Available: {basketItem.Product.Stock}");

            basketItem.Quantity = updateBasketItemDto.Quantity;
            _basketRepository.Update(basketItem);
            await _basketRepository.SaveAsync();

            return _mapper.Map<BasketItemDto>(basketItem);
        }

        public async Task<bool> RemoveFromBasketAsync(int id)
        {
            var basketItem = await _basketRepository.GetByIdAsync(id);
            if (basketItem == null)
                return false;

            _basketRepository.Remove(basketItem);
            await _basketRepository.SaveAsync();
            return true;
        }

        public async Task<bool> ClearBasketAsync(string sessionId, string? userId = null)
        {
            var basketItems = await _basketRepository.GetWhere(x => 
                (x.SessionId == sessionId && x.UserId == null) || 
                (userId != null && x.UserId == userId))
                .ToListAsync();

            if (!basketItems.Any())
                return false;

            _basketRepository.RemoveRange(basketItems);
            await _basketRepository.SaveAsync();
            return true;
        }

        public async Task<int> GetBasketItemCountAsync(string sessionId, string? userId = null)
        {
            var count = await _basketRepository.GetWhere(x => 
                (x.SessionId == sessionId && x.UserId == null) || 
                (userId != null && x.UserId == userId))
                .SumAsync(x => x.Quantity);
            
            return count;
        }

        public async Task<decimal> GetBasketTotalAsync(string sessionId, string? userId = null)
        {
            var total = await _basketRepository.GetWhere(x => 
                (x.SessionId == sessionId && x.UserId == null) || 
                (userId != null && x.UserId == userId))
                .SumAsync(x => x.Price * x.Quantity);
            
            return total;
        }

        public async Task TransferBasketToUserAsync(string sessionId, string userId)
        {
            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(userId))
                return;

            // Get session-based basket items that don't have a user ID
            var sessionItems = await _basketRepository.GetWhere(x => 
                x.SessionId == sessionId && x.UserId == null)
                .ToListAsync();

            if (!sessionItems.Any())
                return;

            // Get existing user basket items to check for duplicates
            var userItems = await _basketRepository.GetWhere(x => x.UserId == userId)
                .ToListAsync();

            foreach (var sessionItem in sessionItems)
            {
                // Check if user already has this product with same attributes
                var existingUserItem = userItems.FirstOrDefault(x =>
                    x.ProductId == sessionItem.ProductId &&
                    x.Size == sessionItem.Size &&
                    x.Color == sessionItem.Color);

                if (existingUserItem != null)
                {
                    // Merge quantities
                    existingUserItem.Quantity += sessionItem.Quantity;
                    _basketRepository.Update(existingUserItem);
                    
                    // Remove the session item
                    _basketRepository.Remove(sessionItem);
                }
                else
                {
                    // Transfer the session item to the user
                    sessionItem.UserId = userId;
                    _basketRepository.Update(sessionItem);
                }
            }

            await _basketRepository.SaveAsync();
        }
    }
}