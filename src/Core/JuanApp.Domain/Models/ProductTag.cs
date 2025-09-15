using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Domain.Models
{
    public class ProductTag
    {
        public int ProductId { get; set; }
        public int TagId { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}