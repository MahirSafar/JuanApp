using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.Areas.Manage.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Slug is required")]
        [StringLength(50, ErrorMessage = "Slug cannot be longer than 50 characters")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase letters, numbers, and hyphens only (e.g., 'my-tag')")]
        public string Slug { get; set; }
    }
}