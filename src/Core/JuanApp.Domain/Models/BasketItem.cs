using JuanApp.Domain.Common;

namespace JuanApp.Domain.Models
{
    public class BasketItem : BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public string SessionId { get; set; }
        public string? UserId { get; set; } // For authenticated users
        
        // Navigation Properties
        public Product Product { get; set; }
        
        public decimal Total => Price * Quantity;
    }
}