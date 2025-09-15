using JuanApp.Domain.Common;

namespace JuanApp.Domain.Models
{
    public class Blog : AuditEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
    }
}
