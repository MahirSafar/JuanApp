namespace JuanApp.Application.Models.ProductDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string Gender { get; set; }
        public string MainImageUrl { get; set; }
        public int CategoryId { get; set; }
        
        // Related Collections
        public List<string> AdditionalImageUrls { get; set; } = new();
        public List<int> SelectedColorIds { get; set; } = new();
        public List<int> SelectedSizeIds { get; set; } = new();
        public List<int> SelectedTagIds { get; set; } = new();
    }
}