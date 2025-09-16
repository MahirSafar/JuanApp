namespace JuanApp.MVC.ViewModels
{
    public class BlogIndexViewModel
    {
        public List<BlogListItemViewModel> Blogs { get; set; } = new();
        public List<BlogListItemViewModel> RecentBlogs { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public string Search { get; set; } = "";
        public int TotalBlogs { get; set; }
    }
    
    public class BlogListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Author { get; set; } = "";
        public string? ImageUrl { get; set; }
        public DateTime Created { get; set; }
    }
    
    public class BlogDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Author { get; set; } = "";
        public string? ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public List<BlogListItemViewModel> RelatedBlogs { get; set; } = new();
    }
}