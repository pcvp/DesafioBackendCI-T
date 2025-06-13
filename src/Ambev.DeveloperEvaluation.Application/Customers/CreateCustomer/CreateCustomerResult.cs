namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Represents the result of the create customer operation.
/// </summary>
public class CreateCustomerResult
{
    /// <summary>
    /// The unique identifier of the created customer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the created customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the created customer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The phone number of the created customer.
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
} 