using AutoMapper;
using JuanApp.Application.Models.ColorDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Concretes
{
    public class ColorService(IGenericRepository<Color> genericRepository, IMapper mapper) : IColorService
    {
        public async Task<bool> CreateAsync(CreateColorDto slider)
        {
            var colorEntity = mapper.Map<Color>(slider);
            await genericRepository.AddAsync(colorEntity);
            return await genericRepository.SaveAsync() > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var color = await genericRepository.GetByIdAsync(id);
            if (color != null)
            {
                genericRepository.Remove(color);
                await genericRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<ColorDto>> GetAllAsync()
        {
            var colors = genericRepository.GetAll();
            return mapper.Map<IEnumerable<ColorDto>>(colors);
        }

        public async Task<ColorDto?> GetByIdAsync(int id)
        {
            var color = await genericRepository.GetByIdAsync(id);   
            return mapper.Map<ColorDto?>(color);
        }

        public async Task UpdateAsync(ColorDto slider)
        {
            var colorEntity = mapper.Map<Color>(slider);
            genericRepository.Update(colorEntity);
            await genericRepository.SaveAsync();
        }
    }
}
