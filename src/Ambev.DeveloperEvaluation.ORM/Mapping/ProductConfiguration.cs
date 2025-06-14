using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity Framework configuration for Product entity
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the Product entity mapping
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        // Indexes
        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Products_Name");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");
    }
} 