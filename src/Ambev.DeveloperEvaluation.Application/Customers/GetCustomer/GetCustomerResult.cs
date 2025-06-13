namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Represents the result of the get customer operation.
/// </summary>
public class GetCustomerResult
{
    /// <summary>
    /// The unique identifier of the customer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the customer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The phone number of the customer.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Indicates whether the customer is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The date and time when the customer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the customer was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 