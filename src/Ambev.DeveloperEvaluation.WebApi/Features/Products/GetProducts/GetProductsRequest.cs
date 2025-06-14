namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Request model for retrieving products with pagination
/// </summary>
public class GetProductsRequest
{
    /// <summary>
    /// Gets or sets the page number (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the optional filter by product name
    /// </summary>
    public string? Name { get; set; }
} 