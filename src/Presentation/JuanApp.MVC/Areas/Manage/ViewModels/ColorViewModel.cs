using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.Areas.Manage.ViewModels
{
    public class ColorViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
        public string Name { get; set; }
    }
}
