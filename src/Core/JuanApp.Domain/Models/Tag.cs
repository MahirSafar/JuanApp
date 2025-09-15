using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Tag : AuditEntity
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Slug { get; set; }

        // Navigation Properties
        public ICollection<ProductTag> ProductTags { get; set; } 
    }
}
