using Microsoft.AspNetCore.Identity;

namespace JuanApp.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? GoogleId { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
