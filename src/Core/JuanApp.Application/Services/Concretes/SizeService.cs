using AutoMapper;
using JuanApp.Application.Models.SizeDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Concretes
{
    public class SizeService(IGenericRepository<Size> genericRepository, IMapper mapper) : ISizeService
    {
        public async Task<bool> CreateAsync(CreateSizeDto size)
        {
            var sizeEntity = mapper.Map<Size>(size);
            await genericRepository.AddAsync(sizeEntity);
            return await genericRepository.SaveAsync() > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var size = await genericRepository.GetByIdAsync(id);
            if (size != null)
            {
                genericRepository.Remove(size);
                await genericRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<SizeDto>> GetAllAsync()
        {
            var sizes = genericRepository.GetAll();
            return mapper.Map<IEnumerable<SizeDto>>(sizes);
        }

        public async Task<SizeDto?> GetByIdAsync(int id)
        {
            var size = await genericRepository.GetByIdAsync(id);   
            return mapper.Map<SizeDto?>(size);
        }

        public async Task UpdateAsync(SizeDto size)
        {
            var sizeEntity = mapper.Map<Size>(size);
            genericRepository.Update(sizeEntity);
            await genericRepository.SaveAsync();
        }
    }
}