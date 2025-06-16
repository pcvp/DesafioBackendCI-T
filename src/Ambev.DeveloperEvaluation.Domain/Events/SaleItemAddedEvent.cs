namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a sale item is added to a sale
/// </summary>
public class SaleItemAddedEvent
{
    /// <summary>
    /// Gets or sets the sale item ID
    /// </summary>
    public Guid SaleItemId { get; set; }

    /// <summary>
    /// Gets or sets the sale ID that this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets when the item was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the event timestamp
    /// </summary>
    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;
} 