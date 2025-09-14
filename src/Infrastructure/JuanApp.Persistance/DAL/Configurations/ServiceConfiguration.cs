using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuanApp.Persistance.DAL.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasData(
              new Service { Id = 1, Title = "FREE SHIPPING", Description = "Free shipping on all order", Icon = "fa-solid fa-truck" },
              new Service { Id = 2, Title = "ONLINE SUPPORT", Description = "Online support 24 hours a day", Icon = "fa-solid fa-signal" },
              new Service { Id = 3, Title = "MONEY RETURN", Description = "Back guarantee under 5 days", Icon = "fa-solid fa-rotate-left" }
            );
        }
    }
}
