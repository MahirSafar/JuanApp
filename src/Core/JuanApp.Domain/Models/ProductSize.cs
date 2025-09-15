using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Domain.Models
{
    public class ProductSize
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("SizeId")]
        public Size Size { get; set; }
    }
}