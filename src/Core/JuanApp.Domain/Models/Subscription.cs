using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        public DateTime SubscribedAt { get; set; }

        public bool IsActive { get; set; }

        public string? Token { get; set; }

        public bool IsConfirmed { get; set; }
    }
}