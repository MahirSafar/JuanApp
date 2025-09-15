using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Domain.Models
{
    public class Product : AuditEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int Stock { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        [StringLength(255)]
        public string MainImageUrl { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }

        // Navigation Properties
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }
}
