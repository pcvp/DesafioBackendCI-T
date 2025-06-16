using Ambev.DeveloperEvaluation.Application.Base;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Handler for processing DeleteCustomerCommand requests
/// </summary>
public class DeleteCustomerHandler : BaseCommandHandler, IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// Initializes a new instance of DeleteCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public DeleteCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Handles the DeleteCustomerCommand request
    /// </summary>
    /// <param name="command">The DeleteCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.Id} not found");

        await _customerRepository.DeleteAsync(command.Id, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit customer deletion transaction");
    }
} 