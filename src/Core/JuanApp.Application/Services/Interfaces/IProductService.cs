using JuanApp.Application.Models.ProductDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateProductDto product);
        Task UpdateAsync(ProductDto product);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> GetByGenderAsync(string gender);
        Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm);
        Task<bool> AddProductImageAsync(int productId, string imageUrl);
        Task<bool> RemoveProductImageAsync(int productImageId);
    }
}