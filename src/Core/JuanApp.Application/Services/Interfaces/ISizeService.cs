using JuanApp.Application.Models.SizeDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface ISizeService
    {
        Task<IEnumerable<SizeDto>> GetAllAsync();
        Task<SizeDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSizeDto size);
        Task UpdateAsync(SizeDto size);
        Task DeleteAsync(int id);
    }
}