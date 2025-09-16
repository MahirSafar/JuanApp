using System.ComponentModel.DataAnnotations;
using JuanApp.Domain.Models;

namespace JuanApp.MVC.ViewModels
{
    public class UserProfileVm
    {
        public UserUpdateProfileVm userUpdateProfile { get; set; }
        public List<UserOrderViewModel> UserOrders { get; set; } = new List<UserOrderViewModel>();
    }

    public class UserOrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }
    }

    public class UserOrderDetailViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<UserOrderItemViewModel> OrderItems { get; set; } = new List<UserOrderItemViewModel>();
    }

    public class UserOrderItemViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal Total { get; set; }
    }
}