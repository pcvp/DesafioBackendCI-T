using MediatR;
using Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Command for updating a sale item
/// </summary>
public class UpdateSaleItemCommand : IRequest<UpdateSaleItemResult>
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