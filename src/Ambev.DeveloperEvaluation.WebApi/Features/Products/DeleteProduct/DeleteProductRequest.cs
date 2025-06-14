namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Request model for deleting a product
/// </summary>
public class DeleteProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to delete
    /// </summary>
    public Guid Id { get; set; }
} 