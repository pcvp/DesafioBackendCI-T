using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItem;

/// <summary>
/// Handler for processing GetSaleItemQuery requests
/// </summary>
public class GetSaleItemHandler : IRequestHandler<GetSaleItemQuery, GetSaleItemResult>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleItemHandler
    /// </summary>
    /// <param name="saleItemRepository">The sale item repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSaleItemHandler(ISaleItemRepository saleItemRepository, IMapper mapper)
    {
        _saleItemRepository = saleItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleItemQuery request
    /// </summary>
    /// <param name="query">The GetSaleItem query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item result</returns>
    public async Task<GetSaleItemResult> Handle(GetSaleItemQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetSaleItemHandler: Retrieving sale item '{query.Id}' from sale '{query.SaleId}'");

        var saleItem = await _saleItemRepository.GetByIdAndSaleIdAsync(query.Id, query.SaleId, cancellationToken);
        
        if (saleItem == null)
            throw new InvalidOperationException($"Sale item with ID {query.Id} not found in sale {query.SaleId}");

        var result = _mapper.Map<GetSaleItemResult>(saleItem);

        Console.WriteLine($"GetSaleItemHandler: Sale item retrieved successfully with product '{result.ProductId}'");

        return result;
    }
} 