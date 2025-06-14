namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request model for retrieving sales with pagination
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Gets or sets the page number (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the search term for filtering sales
    /// </summary>
    public string? Search { get; set; }
} 