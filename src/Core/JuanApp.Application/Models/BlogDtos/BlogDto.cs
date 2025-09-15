namespace JuanApp.Application.Models.BlogDtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}