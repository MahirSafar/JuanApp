namespace JuanApp.MVC.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
        public decimal SubTotal => Items.Sum(x => x.Total);
        public decimal Total => SubTotal; // Simplified - only subtotal
        public int ItemCount => Items.Sum(x => x.Quantity);
    }

    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Total => Price * Quantity;
        public int Stock { get; set; }
    }

    public class AddToBasketViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Size { get; set; }
        public string? Color { get; set; }
    }

    public class UpdateBasketItemViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}