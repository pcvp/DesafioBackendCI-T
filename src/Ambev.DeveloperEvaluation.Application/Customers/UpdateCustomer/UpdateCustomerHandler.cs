using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Handler for processing UpdateCustomerCommand requests
/// </summary>
public class UpdateCustomerHandler : BaseCommandHandler, IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    public UpdateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateCustomerCommand request
    /// </summary>
    /// <param name="command">The UpdateCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer result</returns>
    public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.Id} not found");

        customer.UpdateContactInfo(command.Name, command.Email, command.Phone);
        customer.UpdatedAt = DateTime.UtcNow;

        var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit customer update transaction");

        var result = _mapper.Map<UpdateCustomerResult>(updatedCustomer);
        return result;
    }
} 