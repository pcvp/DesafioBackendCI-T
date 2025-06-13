using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Handler for processing CreateCustomerCommand requests
/// </summary>
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateCustomerCommand request
    /// </summary>
    /// <param name="command">The CreateCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer details</returns>
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"CreateCustomerHandler: Creating customer with name '{command.Name}', email '{command.Email}', phone '{command.Phone}'");

        var validator = new CreateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Check if customer with same email already exists
        if (!string.IsNullOrEmpty(command.Email))
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingCustomer != null)
                throw new InvalidOperationException($"Customer with email {command.Email} already exists");
        }

        // Create customer entity
        var customer = new Customer();
        customer.UpdateContactInfo(command.Name, command.Email, command.Phone);

        // Validate domain entity
        var domainValidation = customer.Validate();
        if (!domainValidation.IsValid)
            throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Save to repository
        var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);
        var result = _mapper.Map<CreateCustomerResult>(createdCustomer);

        Console.WriteLine($"CreateCustomerHandler: Customer created successfully with ID '{result.Id}'");

        return result;
    }
} 