namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;

/// <summary>
/// Response model for GetCustomers operation
/// </summary>
public class GetCustomersResponse
{
    /// <summary>
    /// List of customers
    /// </summary>
    public List<CustomerSummary> Customers { get; set; } = new();

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Total number of customers
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Indicates if there are more pages
    /// </summary>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Indicates if there are previous pages
    /// </summary>
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// Summary information about a customer
/// </summary>
public class CustomerSummary
{
    /// <summary>
    /// The unique identifier of the customer
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The customer name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The customer email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The customer phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Indicates whether the customer is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The date and time when the customer was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 