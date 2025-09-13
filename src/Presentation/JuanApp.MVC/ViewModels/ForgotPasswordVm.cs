using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class ForgotPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
