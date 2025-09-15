using JuanApp.Application.Models.ColorDtos;
using JuanApp.Application.Models.ServiceDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<ColorDto>> GetAllAsync();
        Task<ColorDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateColorDto slider);
        Task UpdateAsync(ColorDto slider);
        Task DeleteAsync(int id);
    }
}
