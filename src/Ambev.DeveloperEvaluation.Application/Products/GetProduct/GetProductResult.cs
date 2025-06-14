namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Result model for retrieving a product
/// </summary>
public class GetProductResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

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
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the product was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the product was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 