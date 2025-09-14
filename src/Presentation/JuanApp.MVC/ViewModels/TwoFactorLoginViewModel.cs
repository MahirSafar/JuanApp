using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class TwoFactorLoginViewModel
    {
        [Required]
        public string TwoFactorCode { get; set; }

        public bool RememberMe { get; set; }
    }
}
