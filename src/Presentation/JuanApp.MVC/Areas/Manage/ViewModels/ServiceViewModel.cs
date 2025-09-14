using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.Areas.Manage.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Icon name is required")]
        public string Icon { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }
    }
}
