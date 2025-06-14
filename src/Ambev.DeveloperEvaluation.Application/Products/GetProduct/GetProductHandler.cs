using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Handler for processing GetProductQuery requests
/// </summary>
public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductQuery request
    /// </summary>
    /// <param name="query">The GetProduct query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product result</returns>
    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var validator = new GetProductQueryValidator();
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(query.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {query.Id} not found");

        return _mapper.Map<GetProductResult>(product);
    }
} 