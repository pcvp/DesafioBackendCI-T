using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItems;

/// <summary>
/// Handler for processing GetSaleItemsQuery requests
/// </summary>
public class GetSaleItemsHandler : IRequestHandler<GetSaleItemsQuery, GetSaleItemsResult>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleItemsHandler
    /// </summary>
    /// <param name="saleItemRepository">The sale item repository</param>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSaleItemsHandler(ISaleItemRepository saleItemRepository, ISaleRepository saleRepository, IMapper mapper)
    {
        _saleItemRepository = saleItemRepository;
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleItemsQuery request
    /// </summary>
    /// <param name="query">The GetSaleItems query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated sale items result</returns>
    public async Task<GetSaleItemsResult> Handle(GetSaleItemsQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetSaleItemsHandler: Retrieving sale items for sale '{query.SaleId}' - Page {query.Page}, Size {query.Size}");

        // Verify that the sale exists
        var sale = await _saleRepository.GetByIdAsync(query.SaleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {query.SaleId} not found");

        // Get paginated sale items
        var (saleItems, totalCount) = await _saleItemRepository.GetBySaleIdPagedAsync(
            query.SaleId, 
            query.Page, 
            query.Size, 
            cancellationToken);

        var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / query.Size);

        var result = new GetSaleItemsResult
        {
            Items = _mapper.Map<List<SaleItemResultDto>>(saleItems),
            CurrentPage = query.Page,
            TotalPages = totalPages,
            TotalCount = totalCount,
            HasNext = totalCount > 0 && query.Page < totalPages,
            HasPrevious = totalCount > 0 && query.Page > 1
        };

        Console.WriteLine($"GetSaleItemsHandler: Retrieved {result.Items.Count} sale items out of {totalCount} total");

        return result;
    }
} 