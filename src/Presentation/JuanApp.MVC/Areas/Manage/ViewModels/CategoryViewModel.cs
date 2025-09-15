using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.Areas.Manage.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; }
    }
}