using MediatR;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;

/// <summary>
/// Command for adding a new item to an existing sale
/// </summary>
public class CreateSaleItemCommand : IRequest<CreateSaleItemResult>
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

    /// <summary>
    /// Gets or sets the discount percentage applied to this item (0-100)
    /// </summary>
    public decimal Discount { get; set; }
} 