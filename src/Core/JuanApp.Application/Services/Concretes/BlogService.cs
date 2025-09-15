using AutoMapper;
using JuanApp.Application.Models.BlogDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Application.Services.Concretes
{
    public class BlogService(IGenericRepository<Blog> genericRepository, IMapper mapper) : IBlogService
    {
        public async Task<bool> CreateAsync(CreateBlogDto blog)
        {
            var blogEntity = mapper.Map<Blog>(blog);
            await genericRepository.AddAsync(blogEntity);
            return await genericRepository.SaveAsync() > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await genericRepository.GetByIdAsync(id);
            if (blog != null)
            {
                genericRepository.Remove(blog);
                await genericRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<BlogDto>> GetAllAsync()
        {
            var blogs = genericRepository.GetAll()
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.Created);
            return mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task<BlogDto?> GetByIdAsync(int id)
        {
            var blog = await genericRepository.GetByIdAsync(id);   
            if (blog != null && !blog.IsDeleted)
            {
                return mapper.Map<BlogDto?>(blog);
            }
            return null;
        }

        public async Task<IEnumerable<BlogDto>> GetRecentBlogsAsync(int count = 5)
        {
            var blogs = genericRepository.GetAll()
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.Created)
                .Take(count);
            return mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task<IEnumerable<BlogDto>> GetBlogsByAuthorAsync(string author)
        {
            var blogs = genericRepository.GetAll()
                .Where(b => !b.IsDeleted && b.Author == author)
                .OrderByDescending(b => b.Created);
            return mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task UpdateAsync(BlogDto blog)
        {
            var blogEntity = mapper.Map<Blog>(blog);
            genericRepository.Update(blogEntity);
            await genericRepository.SaveAsync();
        }
    }
}