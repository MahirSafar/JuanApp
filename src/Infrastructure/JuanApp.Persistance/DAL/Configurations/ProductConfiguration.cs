//using JuanApp.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace JuanApp.Persistance.DAL.Configurations
//{
//    public class ProductConfiguration : IEntityTypeConfiguration<Product>
//    {
//        public void Configure(EntityTypeBuilder<Product> builder)
//        {
//            builder.HasData(
//                new Product
//                {
//                    Id = 1,
//                    Name = "Running Shoe",
//                    Price = 99.99m,
//                    Discount = 10.00m,
//                    Description = "Lightweight and breathable running shoe for performance and comfort.",
//                    Stock = 50,
//                    Gender = "Men",
//                    MainImageUrl = "https://example.com/images/runningshoe_men.jpg",
//                    CategoryId = 1
//                },
//                new Product
//                {
//                    Id = 2,
//                    Name = "Yoga Mat",
//                    Price = 29.99m,
//                    Discount = 5.00m,
//                    Description = "Durable and non-slip yoga mat, perfect for all types of yoga and fitness routines.",
//                    Stock = 75,
//                    Gender = "Unisex",
//                    MainImageUrl = "https://example.com/images/yogamat_unisex.jpg",
//                    CategoryId = 2
//                },
//                new Product
//                {
//                    Id = 3,
//                    Name = "Sports Bra",
//                    Price = 39.50m,
//                    Discount = 0.00m,
//                    Description = "High-impact sports bra with moisture-wicking fabric for maximum support.",
//                    Stock = 100,
//                    Gender = "Women",
//                    MainImageUrl = "https://example.com/images/sportsbra_women.jpg",
//                    CategoryId = 1
//                },
//                new Product
//                {
//                    Id = 4,
//                    Name = "Weighted Jump Rope",
//                    Price = 19.99m,
//                    Discount = 2.50m,
//                    Description = "Adjustable weighted jump rope for an intense cardio workout.",
//                    Stock = 40,
//                    Gender = "Unisex",
//                    MainImageUrl = "https://example.com/images/jumprope_unisex.jpg",
//                    CategoryId = 2
//                },
//                new Product
//                {
//                    Id = 5,
//                    Name = "Hiking Backpack",
//                    Price = 125.00m,
//                    Discount = 15.00m,
//                    Description = "Durable and spacious hiking backpack with multiple compartments and a hydration pouch.",
//                    Stock = 25,
//                    Gender = "Unisex",
//                    MainImageUrl = "https://example.com/images/backpack_unisex.jpg",
//                    CategoryId = 3
//                }
//            );
//        }
//    }
//}
