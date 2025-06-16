using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Handler for processing CreateCustomerCommand requests
/// </summary>
public class CreateCustomerHandler : BaseCommandHandler, IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    public CreateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateCustomerCommand request
    /// </summary>
    /// <param name="command">The CreateCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer result</returns>
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingCustomer = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingCustomer != null)
            throw new InvalidOperationException($"Customer with email {command.Email} already exists");

        var customer = _mapper.Map<Customer>(command);
        var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit customer creation transaction");

        var result = _mapper.Map<CreateCustomerResult>(createdCustomer);
        return result;
    }
} 