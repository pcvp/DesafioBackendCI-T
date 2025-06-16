using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a sale is cancelled
/// </summary>
public class SaleCancelledEvent
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the total amount that was cancelled
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets when the sale was cancelled
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the event timestamp
    /// </summary>
    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;
} 