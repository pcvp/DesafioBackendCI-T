using AutoMapper;
using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Handler for processing GetProductsQuery requests
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductsHandler
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductsHandler(IMapper mapper)
    {
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

        // TODO: Implementar lógica real quando domain layer estiver pronto
        // Por enquanto, retorna dados mock
        var mockProducts = GenerateMockProducts(query.Search);
        var totalCount = mockProducts.Count;
        var totalPages = (int)Math.Ceiling((double)totalCount / query.Size);
        var skip = (query.Page - 1) * query.Size;
        var products = mockProducts.Skip(skip).Take(query.Size).ToList();

        var result = new GetProductsResult
        {
            Products = products,
            CurrentPage = query.Page,
            TotalPages = totalPages,
            TotalCount = totalCount,
            HasNext = query.Page < totalPages,
            HasPrevious = query.Page > 1
        };

        Console.WriteLine($"GetProductsHandler: Retrieved {products.Count} products out of {totalCount} total");

        return result;
    }

    private static List<ProductDto> GenerateMockProducts(string? search)
    {
        var products = new List<ProductDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Laptop Dell", Price = 2500.00m, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new() { Id = Guid.NewGuid(), Name = "Mouse Logitech", Price = 50.00m, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-8) },
            new() { Id = Guid.NewGuid(), Name = "Teclado Mecânico", Price = 150.00m, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new() { Id = Guid.NewGuid(), Name = "Monitor Samsung", Price = 800.00m, IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { Id = Guid.NewGuid(), Name = "Headset Gamer", Price = 200.00m, IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-1) }
        };

        if (!string.IsNullOrEmpty(search))
        {
            products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return products;
    }
} 