namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleItem;

/// <summary>
/// Request model for updating a sale item
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale item to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to this item (0-100)
    /// </summary>
    public decimal Discount { get; set; }
} 