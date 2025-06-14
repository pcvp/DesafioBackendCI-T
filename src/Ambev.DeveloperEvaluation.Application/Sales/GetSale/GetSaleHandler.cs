using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleQuery requests
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleQuery request
    /// </summary>
    /// <param name="query">The GetSale query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale result if found</returns>
    public async Task<GetSaleResult> Handle(GetSaleQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetSaleHandler: Retrieving sale with ID '{query.Id}'");

        if (query.Id == Guid.Empty)
        {
            throw new ArgumentException("Sale ID cannot be empty", nameof(query.Id));
        }

        var sale = await _saleRepository.GetByIdAsync(query.Id, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {query.Id} not found");
        }

        var result = _mapper.Map<GetSaleResult>(sale);

        Console.WriteLine($"GetSaleHandler: Sale retrieved successfully with number '{result.SaleNumber}'");

        return result;
    }
} 