namespace JuanApp.Application.Models.BasketDtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public string SessionId { get; set; }
        public string? UserId { get; set; }
        public decimal Total => Price * Quantity;
        public int Stock { get; set; }
    }

    public class CreateBasketItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string SessionId { get; set; }
        public string? UserId { get; set; }
    }

    public class UpdateBasketItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}