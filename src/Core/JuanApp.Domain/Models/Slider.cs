using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Slider : AuditEntity
    {
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = null!;

        [StringLength(255)]
        public string? RedirectUrl { get; set; }

        [Range(0, 100)]
        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}