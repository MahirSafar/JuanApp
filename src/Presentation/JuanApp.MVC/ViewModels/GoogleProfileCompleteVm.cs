using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class GoogleProfileCompleteVm
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Hidden fields to maintain Google info
        public string Email { get; set; }
        public string GoogleId { get; set; }
    }
}