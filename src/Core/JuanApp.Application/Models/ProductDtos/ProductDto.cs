namespace JuanApp.Application.Models.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string Gender { get; set; }
        public string MainImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        
        // Related Collections
        public List<ProductImageDto> ProductImages { get; set; } = new();
        public List<int> SelectedColorIds { get; set; } = new();
        public List<int> SelectedSizeIds { get; set; } = new();
        public List<int> SelectedTagIds { get; set; } = new();
    }
}