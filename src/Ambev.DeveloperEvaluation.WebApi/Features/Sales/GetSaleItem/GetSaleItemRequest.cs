namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItem;

/// <summary>
/// Request model for retrieving a specific sale item
/// </summary>
public class GetSaleItemRequest
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the sale item ID
    /// </summary>
    public Guid Id { get; set; }
} 