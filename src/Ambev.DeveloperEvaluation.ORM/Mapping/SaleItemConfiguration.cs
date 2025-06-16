using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for SaleItem entity mapping to database
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    /// <summary>
    /// Configures the SaleItem entity mapping
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.SaleId).IsRequired().HasColumnType("uuid");
        builder.Property(si => si.ProductId).IsRequired().HasColumnType("uuid");
        builder.Property(si => si.Quantity).IsRequired();
        builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(si => si.Discount).IsRequired().HasColumnType("decimal(5,2)");
        builder.Property(si => si.TotalAmount).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(si => si.IsCancelled).IsRequired().HasDefaultValue(false);
        builder.Property(si => si.CreatedAt).IsRequired();
        builder.Property(si => si.UpdatedAt);

        // Create index on SaleId for better query performance
        builder.HasIndex(si => si.SaleId).HasDatabaseName("IX_SaleItems_SaleId");

        // Create index on ProductId for product-based queries
        builder.HasIndex(si => si.ProductId).HasDatabaseName("IX_SaleItems_ProductId");

        // Create composite index on SaleId and IsCancelled for filtering
        builder.HasIndex(si => new { si.SaleId, si.IsCancelled }).HasDatabaseName("IX_SaleItems_SaleId_IsCancelled");

        // Create foreign key relationship with Sale entity
        builder.HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 