namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

/// <summary>
/// Represents a request to create a new customer in the system.
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>
    /// Gets or sets the customer name. Must not be empty.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address. Must be a valid email format if provided.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number. Optional field.
    /// </summary>
    public string? Phone { get; set; }
} 