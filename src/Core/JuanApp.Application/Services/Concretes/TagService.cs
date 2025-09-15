using AutoMapper;
using JuanApp.Application.Models.TagDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Concretes
{
    public class TagService(IGenericRepository<Tag> genericRepository, IMapper mapper) : ITagService
    {
        public async Task<bool> CreateAsync(CreateTagDto tag)
        {
            var tagEntity = mapper.Map<Tag>(tag);
            await genericRepository.AddAsync(tagEntity);
            return await genericRepository.SaveAsync() > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await genericRepository.GetByIdAsync(id);
            if (tag != null)
            {
                genericRepository.Remove(tag);
                await genericRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = genericRepository.GetAll();
            return mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(int id)
        {
            var tag = await genericRepository.GetByIdAsync(id);   
            return mapper.Map<TagDto?>(tag);
        }

        public async Task UpdateAsync(TagDto tag)
        {
            var tagEntity = mapper.Map<Tag>(tag);
            genericRepository.Update(tagEntity);
            await genericRepository.SaveAsync();
        }
    }
}