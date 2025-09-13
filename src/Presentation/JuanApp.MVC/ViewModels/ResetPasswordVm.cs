using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class ResetPasswordVm
    {
        public string Token { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords must be same")]
        public string ConfirmPassword { get; set; }
    }
}
