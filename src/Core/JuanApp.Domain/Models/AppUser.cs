using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        public string FullName { get; set; }

        public string? GoogleId { get; set; }

        public bool IsSubscribed { get; set; }

        // Navigation Properties
        public ICollection<Review> Reviews { get; set; }
    }
}
