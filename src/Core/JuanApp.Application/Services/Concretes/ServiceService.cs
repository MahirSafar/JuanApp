using AutoMapper;
using JuanApp.Application.Models.ServiceDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Concretes
{
    public class ServiceService(IGenericRepository<Service> genericRepository, IMapper mapper) : IServiceService
    {
        public async Task<bool> CreateAsync(CreateServiceDto slider)
        {
            var serviceEntity = mapper.Map<Service>(slider);
            await genericRepository.AddAsync(serviceEntity);
            return await genericRepository.SaveAsync() > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var service = await genericRepository.GetByIdAsync(id);
            if (service != null)
            {
                genericRepository.Remove(service);
                await genericRepository.SaveAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await genericRepository.AnyAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var services = genericRepository.GetAll();
            return mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var slider = await genericRepository.GetByIdAsync(id);
            return mapper.Map<ServiceDto?>(slider);
        }

        public async Task UpdateAsync(ServiceDto slider)
        {
            var sliderEntity = mapper.Map<Service>(slider);
            genericRepository.Update(sliderEntity);
            await genericRepository.SaveAsync();
        }
    }
}
