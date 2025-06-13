using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Handler for processing DeleteCustomerCommand requests
/// </summary>
public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// Initializes a new instance of DeleteCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    public DeleteCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Handles the DeleteCustomerCommand request
    /// </summary>
    /// <param name="command">The DeleteCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the operation</returns>
    public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"DeleteCustomerHandler: Deleting customer with ID '{command.Id}'");

        var deleted = await _customerRepository.DeleteAsync(command.Id, cancellationToken);
        
        if (!deleted)
            throw new InvalidOperationException($"Customer with ID {command.Id} not found");

        Console.WriteLine($"DeleteCustomerHandler: Customer with ID '{command.Id}' deleted successfully");
    }
} 