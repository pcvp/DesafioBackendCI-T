namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSaleItem;

/// <summary>
/// Request model for deleting a sale item
/// </summary>
public class DeleteSaleItemRequest
{
    /// <summary>
    /// Gets or sets the sale item ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale ID that this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }
} 