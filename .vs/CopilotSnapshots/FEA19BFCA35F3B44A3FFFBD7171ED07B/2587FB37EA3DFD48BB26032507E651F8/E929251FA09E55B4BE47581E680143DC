using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Color : AuditEntity
    {
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<ProductColor> ProductColors { get; set; }
    }
}
