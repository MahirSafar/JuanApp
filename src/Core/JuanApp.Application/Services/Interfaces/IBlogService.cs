using JuanApp.Application.Models.BlogDtos;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogDto>> GetAllAsync();
        Task<BlogDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateBlogDto blog);
        Task UpdateAsync(BlogDto blog);
        Task DeleteAsync(int id);
        Task<IEnumerable<BlogDto>> GetRecentBlogsAsync(int count = 5);
        Task<IEnumerable<BlogDto>> GetBlogsByAuthorAsync(string author);
    }
}