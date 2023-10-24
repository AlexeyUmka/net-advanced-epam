using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.CategoryId)
            .IsRequired();
        
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired();
    }
}
