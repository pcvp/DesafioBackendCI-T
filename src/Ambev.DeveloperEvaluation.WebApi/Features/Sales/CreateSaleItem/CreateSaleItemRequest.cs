namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem;

/// <summary>
/// Request model for adding a new item to an existing sale
/// </summary>
public class AddSaleItemRequest
{
    /// <summary>
    /// Gets or sets the sale ID that this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }
} 