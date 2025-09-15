using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Domain.Models
{
    public class ProductImage : AuditEntity
    {
        [StringLength(255)]
        public string ImageUrl { get; set; }

        // Foreign Key
        public int ProductId { get; set; }

        // Navigation Property
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
