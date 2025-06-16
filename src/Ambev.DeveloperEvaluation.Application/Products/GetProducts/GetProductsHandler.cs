using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Handler for processing GetProductsQuery requests
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductsHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductsQuery request
    /// </summary>
    /// <param name="query">The GetProducts query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated products result</returns>
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"GetProductsHandler: Retrieving products - Page: {query.Page}, Size: {query.Size}, Search: '{query.Search}'");

        var validator = new GetProductsQueryValidator();
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var (products, totalCount) = await _productRepository.GetPagedAsync(query.Page, query.Size, query.Search, cancellationToken);
        
        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / query.Size);

        var result = new GetProductsResult
        {
            Products = productDtos,
            CurrentPage = query.Page,
            TotalPages = totalPages,
            TotalCount = totalCount,
            HasNext = query.Page < totalPages,
            HasPrevious = query.Page > 1
        };

        Console.WriteLine($"GetProductsHandler: Retrieved {productDtos.Count} products out of {totalCount} total");

        return result;
    }
} 