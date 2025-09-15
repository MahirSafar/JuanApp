namespace JuanApp.Application.Models.BlogDtos
{
    public class CreateBlogDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; } // Required - no longer nullable
    }
}