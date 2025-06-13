using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

/// <summary>
/// Handler for processing GetCustomersCommand requests
/// </summary>
public class GetCustomersHandler : IRequestHandler<GetCustomersCommand, GetCustomersResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCustomersHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCustomersCommand request
    /// </summary>
    /// <param name="command">The GetCustomers command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated customers list</returns>
    public async Task<GetCustomersResult> Handle(GetCustomersCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetCustomersHandler: Retrieving customers - Page: {command.Page}, Size: {command.Size}, Name: '{command.Name}', Email: '{command.Email}'");

        var (customers, totalCount) = await _customerRepository.GetPagedAsync(
            command.Page, 
            command.Size, 
            command.Name, 
            command.Email, 
            cancellationToken);

        var customerSummaries = customers.Select(c => _mapper.Map<CustomerSummaryResult>(c)).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / command.Size);

        var result = new GetCustomersResult
        {
            Customers = customerSummaries,
            CurrentPage = command.Page,
            TotalPages = totalPages,
            TotalCount = totalCount,
            HasNextPage = command.Page < totalPages,
            HasPreviousPage = command.Page > 1
        };

        Console.WriteLine($"GetCustomersHandler: Retrieved {result.Customers.Count} customers successfully");

        return result;
    }
} 