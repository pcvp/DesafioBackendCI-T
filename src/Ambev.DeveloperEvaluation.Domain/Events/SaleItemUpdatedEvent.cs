namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a sale item is updated
/// </summary>
public class SaleItemUpdatedEvent
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
    /// Gets or sets the previous product ID
    /// </summary>
    public Guid PreviousProductId { get; set; }

    /// <summary>
    /// Gets or sets the new product ID
    /// </summary>
    public Guid NewProductId { get; set; }

    /// <summary>
    /// Gets or sets the previous quantity
    /// </summary>
    public int PreviousQuantity { get; set; }

    /// <summary>
    /// Gets or sets the new quantity
    /// </summary>
    public int NewQuantity { get; set; }

    /// <summary>
    /// Gets or sets the previous unit price
    /// </summary>
    public decimal PreviousUnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the new unit price
    /// </summary>
    public decimal NewUnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the previous discount
    /// </summary>
    public decimal PreviousDiscount { get; set; }

    /// <summary>
    /// Gets or sets the new discount
    /// </summary>
    public decimal NewDiscount { get; set; }

    /// <summary>
    /// Gets or sets the previous total amount
    /// </summary>
    public decimal PreviousTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the new total amount
    /// </summary>
    public decimal NewTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets when the item was updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the event timestamp
    /// </summary>
    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;
} 