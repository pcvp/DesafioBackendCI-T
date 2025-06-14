namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Request model for creating a new product
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }
} 