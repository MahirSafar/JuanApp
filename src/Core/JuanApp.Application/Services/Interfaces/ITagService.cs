using JuanApp.Application.Models.TagDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateTagDto tag);
        Task UpdateAsync(TagDto tag);
        Task DeleteAsync(int id);
    }
}