using JuanApp.Application.Services.Interfaces;
using JuanApp.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.MVC.Controllers
{
    public class BlogController(IBlogService blogService) : Controller
    {
        private const int PageSize = 6;
        
        public async Task<IActionResult> Index(string search = "", int page = 1)
        {
            var blogs = await blogService.GetAllAsync();
            
            // Filter blogs based on search
            if (!string.IsNullOrEmpty(search))
            {
                blogs = blogs.Where(b => 
                    b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    b.Content.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            
            var blogList = blogs.OrderByDescending(b => b.Created).ToList();
            
            // Get recent blogs for sidebar
            var recentBlogs = await blogService.GetRecentBlogsAsync(5);
            
            var paginatedBlogs = blogList
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(b => new BlogListItemViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    Created = b.Created
                }).ToList();
            
            var viewModel = new BlogIndexViewModel
            {
                Blogs = paginatedBlogs,
                RecentBlogs = recentBlogs.Select(b => new BlogListItemViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Created = b.Created
                }).ToList(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)blogList.Count / PageSize),
                HasPreviousPage = page > 1,
                HasNextPage = page < (int)Math.Ceiling((double)blogList.Count / PageSize),
                Search = search,
                TotalBlogs = blogList.Count
            };
            
            return View(viewModel);
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var blog = await blogService.GetByIdAsync(id);
            if (blog == null)
                return NotFound();
            
            // Get related blogs by the same author (excluding current blog)
            var relatedBlogs = await blogService.GetBlogsByAuthorAsync(blog.Author);
            var relatedBlogsList = relatedBlogs
                .Where(b => b.Id != id)
                .OrderByDescending(b => b.Created)
                .Take(5)
                .Select(b => new BlogListItemViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Created = b.Created,
                    ImageUrl = b.ImageUrl
                }).ToList();
            
            var viewModel = new BlogDetailViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.Author,
                ImageUrl = blog.ImageUrl,
                Created = blog.Created,
                RelatedBlogs = relatedBlogsList
            };
            
            return View(viewModel);
        }
    }
}