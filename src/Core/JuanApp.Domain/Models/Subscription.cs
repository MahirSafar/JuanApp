using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        public DateTime SubscribedAt { get; set; }
        
        public bool IsActive { get; set; }
        
        public string? Token { get; set; }
        
        public bool IsConfirmed { get; set; }
    }
}