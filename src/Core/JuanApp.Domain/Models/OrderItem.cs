using JuanApp.Domain.Common;

namespace JuanApp.Domain.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        
        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
        
        public decimal Total => Price * Quantity;
    }
}