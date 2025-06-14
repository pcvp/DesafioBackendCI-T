namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Response model for retrieving products with pagination
/// </summary>
public class GetProductsResponse
{
    /// <summary>
    /// Gets or sets the list of products
    /// </summary>
    public List<ProductDto> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of products
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets whether there is a next page
    /// </summary>
    public bool HasNext { get; set; }

    /// <summary>
    /// Gets or sets whether there is a previous page
    /// </summary>
    public bool HasPrevious { get; set; }
}

/// <summary>
/// Product data transfer object
/// </summary>
public class ProductDto
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