using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Domain.Models
{
    public class Size : AuditEntity
    {
        [Column(TypeName = "decimal(5, 2)")]
        public decimal ShoeSize { get; set; }

        // Navigation Properties
        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}
