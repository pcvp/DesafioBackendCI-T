using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Customer entity mapping to database
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <summary>
    /// Configures the Customer entity mapping
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt);

        // Create unique index on email (when not null)
        builder.HasIndex(c => c.Email).IsUnique().HasFilter("\"Email\" IS NOT NULL");
    }
} 