namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

/// <summary>
/// Response model for UpdateCustomer operation
/// </summary>
public class UpdateCustomerResponse
{
    /// <summary>
    /// The unique identifier of the updated customer
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The updated customer name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The updated customer email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The updated customer phone number
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
    public DateTime UpdatedAt { get; set; }
} 