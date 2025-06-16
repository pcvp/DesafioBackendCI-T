namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItems;

/// <summary>
/// Request model for retrieving sale items with pagination
/// </summary>
public class GetSaleItemsRequest
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the page number (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page
    /// </summary>
    public int Size { get; set; } = 10;
} 