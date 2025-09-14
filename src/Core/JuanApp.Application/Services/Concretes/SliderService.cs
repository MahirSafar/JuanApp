using AutoMapper;
using JuanApp.Application.Models;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Application.Services.Concretes;

public class SliderService(IGenericRepository<Slider> repository, IMapper mapper) : ISliderService
{

    public async Task<IEnumerable<SliderDto>> GetAllAsync()
    {
        var sliders = await repository.GetAll()
                             .OrderBy(s => s.Order)
                             .ToListAsync();
        return mapper.Map<IEnumerable<SliderDto>>(sliders);
    }

    public async Task<SliderDto?> GetByIdAsync(int id)
    {
        var slider = await repository.GetByIdAsync(id); 
        return mapper.Map<SliderDto?>(slider);
    }

    public async Task<bool> CreateAsync(CreateSliderDto slider)
    {
        var sliderEntity = mapper.Map<Slider>(slider);
        await repository.AddAsync(sliderEntity);
        return await repository.SaveAsync() > 0;
    }
    public async Task UpdateAsync(SliderDto slider)
    {
        var sliderEntity = mapper.Map<Slider>(slider);
        repository.Update(sliderEntity);
        await repository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var slider = await repository.GetByIdAsync(id);

        if (slider != null)
        {
            repository.Remove(slider);
            await repository.SaveAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await repository.AnyAsync(s => s.Id == id);
    }

    public async Task<int> GetMaxOrderAsync()
    {
        var maxOrder = await repository.GetAll()
                                     .MaxAsync(s => (int?)s.Order);
        return maxOrder ?? 0;
    }
}