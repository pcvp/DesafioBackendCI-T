using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Command for retrieving a customer by ID.
/// </summary>
public class GetCustomerCommand : IRequest<GetCustomerResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer to retrieve.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of GetCustomerCommand.
    /// </summary>
    /// <param name="id">The customer ID</param>
    public GetCustomerCommand(Guid id)
    {
        Id = id;
    }
} 