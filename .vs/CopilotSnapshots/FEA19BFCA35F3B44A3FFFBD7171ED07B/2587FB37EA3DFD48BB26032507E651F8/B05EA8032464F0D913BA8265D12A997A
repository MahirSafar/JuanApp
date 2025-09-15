using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Category : AuditEntity
    {
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
