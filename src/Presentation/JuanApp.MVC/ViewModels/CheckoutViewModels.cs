using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string CustomerName { get; set; }
        
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string CustomerSurname { get; set; }
        
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        // For displaying basket items during checkout
        public List<BasketItemViewModel> BasketItems { get; set; } = new List<BasketItemViewModel>();
        public decimal TotalAmount { get; set; }
    }

    public class CheckoutSuccessViewModel
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal Total { get; set; }
    }
}