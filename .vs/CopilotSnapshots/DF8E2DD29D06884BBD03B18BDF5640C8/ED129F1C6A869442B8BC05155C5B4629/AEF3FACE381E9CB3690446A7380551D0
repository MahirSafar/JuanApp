using JuanApp.Domain.Common;
using JuanApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Domain.Models
{
    public class Review : AuditEntity
    {
        [StringLength(500)]
        public string Content { get; set; }

        [Range(1, 5)]
        public int Rate { get; set; }

        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        // Foreign Keys
        public int ProductId { get; set; }
        public string AppUserId { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
