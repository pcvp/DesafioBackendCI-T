namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

/// <summary>
/// Request model for updating a customer
/// </summary>
public class UpdateCustomerRequest
{
    /// <summary>
    /// The unique identifier of the customer to update
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
    /// Indicates whether the customer should be active
    /// </summary>
    public bool IsActive { get; set; } = true;
} 