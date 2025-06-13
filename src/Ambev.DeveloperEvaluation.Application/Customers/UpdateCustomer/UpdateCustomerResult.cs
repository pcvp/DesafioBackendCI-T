namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Represents the result of the update customer operation.
/// </summary>
public class UpdateCustomerResult
{
    /// <summary>
    /// The unique identifier of the updated customer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The updated name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The updated email address of the customer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The updated phone number of the customer.
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
    public DateTime UpdatedAt { get; set; }
} 