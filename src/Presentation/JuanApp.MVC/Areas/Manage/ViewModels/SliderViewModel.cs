using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.Areas.Manage.ViewModels
{
    public class SliderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Redirect URL")]
        public string? RedirectUrl { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Order must be a positive number")]
        public int Order { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}