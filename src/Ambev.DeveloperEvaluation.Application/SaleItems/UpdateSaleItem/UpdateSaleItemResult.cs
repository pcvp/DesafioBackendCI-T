namespace Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

/// <summary>
/// Result for updating a sale item
/// </summary>
public class UpdateSaleItemResult
{
    /// <summary>
    /// Gets or sets the sale item ID
    /// </summary>
    public Guid Id { get; set; }

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
    /// Gets or sets the unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to this item (0-100)
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (calculated)
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the item was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the item was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 