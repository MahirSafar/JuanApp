using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuanApp.Persistance.DAL.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.SessionId)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.UserId)
                .HasMaxLength(450);

            builder.Property(x => x.Size)
                .HasMaxLength(50);

            builder.Property(x => x.Color)
                .HasMaxLength(50);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(x => x.SessionId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.SessionId, x.ProductId });
        }
    }
}