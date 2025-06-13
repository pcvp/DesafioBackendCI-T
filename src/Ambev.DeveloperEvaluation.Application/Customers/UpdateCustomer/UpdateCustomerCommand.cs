using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Command for updating an existing customer.
/// </summary>
public class UpdateCustomerCommand : IRequest<UpdateCustomerResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated email address for the customer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the updated phone number for the customer.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets whether the customer should be active.
    /// </summary>
    public bool IsActive { get; set; } = true;
} 