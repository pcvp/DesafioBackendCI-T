using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Handler for processing UpdateCustomerCommand requests
/// </summary>
public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateCustomerCommand request
    /// </summary>
    /// <param name="command">The UpdateCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer details</returns>
    public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"UpdateCustomerHandler: Updating customer with ID '{command.Id}' - Name: '{command.Name}', Email: '{command.Email}', Phone: '{command.Phone}', IsActive: {command.IsActive}");

        var validator = new UpdateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Get existing customer
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (customer == null)
            throw new InvalidOperationException($"Customer with ID {command.Id} not found");

        // Check if email is being changed and if new email already exists
        if (!string.IsNullOrEmpty(command.Email) && command.Email != customer.Email)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingCustomer != null && existingCustomer.Id != command.Id)
                throw new InvalidOperationException($"Customer with email {command.Email} already exists");
        }

        // Update customer
        customer.UpdateContactInfo(command.Name, command.Email, command.Phone);
        if (command.IsActive != customer.IsActive)
        {
            if (command.IsActive)
                customer.Activate();
            else
                customer.Deactivate();
        }

        // Validate domain entity
        var domainValidation = customer.Validate();
        if (!domainValidation.IsValid)
            throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Save changes
        var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);
        var result = _mapper.Map<UpdateCustomerResult>(updatedCustomer);

        Console.WriteLine($"UpdateCustomerHandler: Customer updated successfully with ID '{result.Id}'");

        return result;
    }
} 