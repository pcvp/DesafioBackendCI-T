namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Constants for event topics
/// </summary>
public static class EventTopics
{
    /// <summary>
    /// Topic for sale created events
    /// </summary>
    public const string SaleCreated = "sale.created";

    /// <summary>
    /// Topic for sale modified events
    /// </summary>
    public const string SaleModified = "sale.modified";

    /// <summary>
    /// Topic for sale cancelled events
    /// </summary>
    public const string SaleCancelled = "sale.cancelled";

    /// <summary>
    /// Topic for sale status changed events
    /// </summary>
    public const string SaleStatusChanged = "sale.status.changed";

    /// <summary>
    /// Topic for sale item cancelled events
    /// </summary>
    public const string SaleItemCancelled = "sale.item.cancelled";
} 