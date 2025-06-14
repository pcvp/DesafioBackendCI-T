using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets whether the product is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets the date and time when the product was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the product's information
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Product class
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the Product class with specified values
    /// </summary>
    /// <param name="name">The product name</param>
    /// <param name="price">The product price</param>
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
        IsActive = true;
    }

    /// <summary>
    /// Updates the product information
    /// </summary>
    /// <param name="name">The new product name</param>
    /// <param name="price">The new product price</param>
    public void Update(string name, decimal price)
    {
        Name = name;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the product
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the product
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
} 