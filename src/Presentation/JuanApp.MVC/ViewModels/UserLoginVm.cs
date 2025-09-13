using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class UserLoginVm
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
