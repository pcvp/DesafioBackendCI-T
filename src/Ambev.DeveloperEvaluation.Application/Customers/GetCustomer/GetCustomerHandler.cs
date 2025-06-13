using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Handler for processing GetCustomerCommand requests
/// </summary>
public class GetCustomerHandler : IRequestHandler<GetCustomerCommand, GetCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCustomerCommand request
    /// </summary>
    /// <param name="command">The GetCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer details</returns>
    public async Task<GetCustomerResult> Handle(GetCustomerCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetCustomerHandler: Retrieving customer with ID '{command.Id}'");

        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (customer == null)
            throw new InvalidOperationException($"Customer with ID {command.Id} not found");

        var result = _mapper.Map<GetCustomerResult>(customer);

        Console.WriteLine($"GetCustomerHandler: Customer retrieved successfully - Name: '{result.Name}'");

        return result;
    }
} 