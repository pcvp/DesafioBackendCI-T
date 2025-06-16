using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a sale status changes
/// </summary>
public class SaleStatusChangedEvent
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
    /// Gets or sets the current status
    /// </summary>
    public SaleStatusEnum Status { get; set; }

    /// <summary>
    /// Gets or sets the total amount at the time of status change
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets when the status was updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the event timestamp
    /// </summary>
    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;
} 