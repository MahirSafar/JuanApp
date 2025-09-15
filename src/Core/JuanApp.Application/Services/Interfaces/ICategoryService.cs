using JuanApp.Application.Models.CategoryDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCategoryDto category);
        Task UpdateAsync(CategoryDto category);
        Task DeleteAsync(int id);
    }
}