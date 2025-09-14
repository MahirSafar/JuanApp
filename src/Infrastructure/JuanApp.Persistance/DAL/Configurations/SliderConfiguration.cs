using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuanApp.Persistance.DAL.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.HasData(
                  new Slider
                  {
                      Id = 1,
                      Title = "Creative and Smart",
                      Description = "Check out our new summer collection of stylish accessories and bags.",
                      ImageUrl = "slider-1.jpg",
                      RedirectUrl = "/shop",
                      Order = 1,
                      IsActive = true
                  },
                  new Slider
                  {
                      Id = 2,
                      Title = "Amazing Fashion",
                      Description = "Explore the latest trends in fashion and find your unique style.",
                      ImageUrl = "slider-2.jpg",
                      RedirectUrl = "/shop",
                      Order = 2,
                      IsActive = true
                  },
                  new Slider
                  {
                      Id = 7,
                      Title = "Unique and Modern",
                      Description = "Discover a curated collection of modern and exclusive products.",
                      ImageUrl = "slider-3.jpg",
                      RedirectUrl = "/shop",
                      Order = 3,
                      IsActive = true
                  }
                );
        }
    }
}
