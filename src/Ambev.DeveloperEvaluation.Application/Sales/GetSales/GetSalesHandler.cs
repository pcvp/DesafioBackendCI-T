using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesQuery requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSalesQuery request
    /// </summary>
    /// <param name="query">The GetSales query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated sales result</returns>
    public async Task<GetSalesResult> Handle(GetSalesQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetSalesHandler: Retrieving sales - Page: {query.Page}, Size: {query.Size}, Search: '{query.Search}'");

        var (sales, totalCount) = await _saleRepository.GetPagedAsync(
            query.Page,
            query.Size,
            query.Search, 
            null, 
            null, 
            null, 
            cancellationToken
        );

        var totalPages = (int)Math.Ceiling((double)totalCount / query.Size);

        var result = new GetSalesResult
        {
            Sales = _mapper.Map<List<SaleResultDto>>(sales),
            CurrentPage = query.Page,
            TotalPages = totalPages,
            TotalCount = totalCount,
            HasNext = query.Page < totalPages,
            HasPrevious = query.Page > 1
        };

        Console.WriteLine($"GetSalesHandler: Retrieved {result.Sales.Count} sales successfully");

        return result;
    }
} 