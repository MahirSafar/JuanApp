using JuanApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.Areas.Manage.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Review Content")]
        public string Content { get; set; } = string.Empty;
        
        [Display(Name = "Rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rate { get; set; }
        
        [Display(Name = "Status")]
        public ReviewStatus Status { get; set; }
        
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        
        [Display(Name = "User")]
        public string AppUserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }
        
        [Display(Name = "Last Modified")]
        public DateTime? LastModified { get; set; }
    }

    public class ReviewFilterViewModel
    {
        public ReviewStatus? Status { get; set; }
        public int? ProductId { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}