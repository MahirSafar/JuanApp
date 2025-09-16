using JuanApp.Application.Models.BasketDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IBasketService
    {
        Task<IEnumerable<BasketItemDto>> GetBasketItemsAsync(string sessionId, string? userId = null);
        Task<BasketItemDto?> GetBasketItemAsync(int id);
        Task<BasketItemDto> AddToBasketAsync(CreateBasketItemDto createBasketItemDto);
        Task<BasketItemDto> UpdateBasketItemAsync(UpdateBasketItemDto updateBasketItemDto);
        Task<bool> RemoveFromBasketAsync(int id);
        Task<bool> ClearBasketAsync(string sessionId, string? userId = null);
        Task<int> GetBasketItemCountAsync(string sessionId, string? userId = null);
        Task<decimal> GetBasketTotalAsync(string sessionId, string? userId = null);
        Task TransferBasketToUserAsync(string sessionId, string userId);
    }
}