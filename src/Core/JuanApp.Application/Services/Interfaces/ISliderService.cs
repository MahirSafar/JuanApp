using JuanApp.Application.Models.SliderDtos;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderDto>> GetAllAsync();
        Task<SliderDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSliderDto slider);
        Task UpdateAsync(SliderDto slider);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetMaxOrderAsync();
    }
}