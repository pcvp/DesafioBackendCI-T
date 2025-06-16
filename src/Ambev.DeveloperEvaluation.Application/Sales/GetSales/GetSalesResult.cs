using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result model for retrieving sales with pagination
/// </summary>
public class GetSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales
    /// </summary>
    public List<SaleResultDto> Sales { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of sales
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets whether there is a next page
    /// </summary>
    public bool HasNext { get; set; }

    /// <summary>
    /// Gets or sets whether there is a previous page
    /// </summary>
    public bool HasPrevious { get; set; }
}

/// <summary>
/// DTO for individual sale in the list
/// </summary>
public class SaleResultDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID where the sale was made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the total amount for the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the sale status
    /// </summary>
    public SaleStatusEnum Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 