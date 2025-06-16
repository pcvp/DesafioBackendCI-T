using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItem;

/// <summary>
/// Query for retrieving a specific sale item
/// </summary>
public class GetSaleItemQuery : IRequest<GetSaleItemResult>
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