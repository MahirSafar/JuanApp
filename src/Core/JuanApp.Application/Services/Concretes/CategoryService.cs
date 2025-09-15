using AutoMapper;
using JuanApp.Application.Models.CategoryDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Concretes
{
    public class CategoryService(IGenericRepository<Category> genericRepository, IMapper mapper) : ICategoryService
    {
        public async Task<bool> CreateAsync(CreateCategoryDto category)
        {
            var categoryEntity = mapper.Map<Category>(category);
            await genericRepository.AddAsync(categoryEntity);
            return await genericRepository.SaveAsync() > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await genericRepository.GetByIdAsync(id);
            if (category != null)
            {
                genericRepository.Remove(category);
                await genericRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = genericRepository.GetAll();
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await genericRepository.GetByIdAsync(id);   
            return mapper.Map<CategoryDto?>(category);
        }

        public async Task UpdateAsync(CategoryDto category)
        {
            var categoryEntity = mapper.Map<Category>(category);
            genericRepository.Update(categoryEntity);
            await genericRepository.SaveAsync();
        }
    }
}