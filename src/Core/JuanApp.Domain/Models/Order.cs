using JuanApp.Domain.Common;

namespace JuanApp.Domain.Models
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        // Navigation Properties
        public AppUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
    
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}