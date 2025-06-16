using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Sale entity mapping to database
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the Sale entity mapping
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
        builder.Property(s => s.SaleDate).IsRequired();
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.BranchId).IsRequired();
        builder.Property(s => s.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(s => s.Status).IsRequired().HasConversion<int>();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt);

        builder.Ignore(s => s.TotalSaleAmount);

        // Configure one-to-many relationship with SaleItems
        builder.HasMany(s => s.Items)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create unique index on sale number
        builder.HasIndex(s => s.SaleNumber).IsUnique();

        // Create indexes for common queries
        builder.HasIndex(s => s.CustomerId);
        builder.HasIndex(s => s.BranchId);
        builder.HasIndex(s => s.SaleDate);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.CreatedAt);

        // Composite indexes for common filter combinations
        builder.HasIndex(s => new { s.CustomerId, s.SaleDate });
        builder.HasIndex(s => new { s.BranchId, s.SaleDate });
        builder.HasIndex(s => new { s.Status, s.SaleDate });
    }
}
