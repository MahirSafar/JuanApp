using System.ComponentModel.DataAnnotations;

namespace JuanApp.MVC.ViewModels
{
    public class ShopIndexViewModel
    {
        public List<ProductItemViewModel> Products { get; set; } = new();
        public List<CategoryItemViewModel> Categories { get; set; } = new();
        public List<ColorItemViewModel> Colors { get; set; } = new();
        public List<SizeItemViewModel> Sizes { get; set; } = new();
        public ShopFilterViewModel Filters { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
        public string ViewMode { get; set; } = "grid-view"; // grid-view or list-view
        
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    public class ShopFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public List<int> ColorIds { get; set; } = new();
        public List<int> SizeIds { get; set; } = new();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Gender { get; set; }
        public string SortBy { get; set; } = "newest"; // newest, price-low-high, price-high-low, name-asc, name-desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public string ViewMode { get; set; } = "grid-view";
    }

    public class PriceRangeViewModel
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}