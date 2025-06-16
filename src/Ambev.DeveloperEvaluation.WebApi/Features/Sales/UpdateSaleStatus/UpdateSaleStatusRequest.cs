using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleStatus;

/// <summary>
/// Request model for updating sale status
/// </summary>
public class UpdateSaleStatusRequest
{
    /// <summary>
    /// Gets or sets the sale ID (set by route parameter)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the new status for the sale
    /// </summary>
    public SaleStatusEnum Status { get; set; }
} 