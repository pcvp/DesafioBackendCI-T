namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

/// <summary>
/// Response model for GetCustomer operation
/// </summary>
public class GetCustomerResponse
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

    /// <summary>
    /// The date and time when the customer was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 