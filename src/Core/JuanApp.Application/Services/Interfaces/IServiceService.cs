using JuanApp.Application.Models;
using JuanApp.Application.Models.ServiceDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetAllAsync();
        Task<ServiceDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateServiceDto slider);
        Task UpdateAsync(ServiceDto slider);
        Task DeleteAsync(int id);
    }
}
