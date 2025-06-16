using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for Product-related operations using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateProductCommand entities.
    /// </summary>
    private static readonly Faker<CreateProductCommand> createProductCommandFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 9999));

    /// <summary>
    /// Configures the Faker to generate valid GetProductQuery entities.
    /// </summary>
    private static readonly Faker<GetProductQuery> getProductQueryFaker = new Faker<GetProductQuery>()
        .RuleFor(p => p.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid GetProductsQuery entities.
    /// </summary>
    private static readonly Faker<GetProductsQuery> getProductsQueryFaker = new Faker<GetProductsQuery>()
        .RuleFor(p => p.Page, f => f.Random.Int(1, 10))
        .RuleFor(p => p.Size, f => f.Random.Int(1, 100))
        .RuleFor(p => p.Search, f => f.Random.Bool() ? f.Commerce.ProductName() : null);

    /// <summary>
    /// Configures the Faker to generate valid UpdateProductCommand entities.
    /// </summary>
    private static readonly Faker<UpdateProductCommand> updateProductCommandFaker = new Faker<UpdateProductCommand>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 9999));

    /// <summary>
    /// Configures the Faker to generate valid DeleteProductCommand entities.
    /// </summary>
    private static readonly Faker<DeleteProductCommand> deleteProductCommandFaker = new Faker<DeleteProductCommand>()
        .RuleFor(p => p.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// </summary>
    private static readonly Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 9999))
        .RuleFor(p => p.IsActive, f => f.Random.Bool())
        .RuleFor(p => p.CreatedAt, f => f.Date.Recent())
        .RuleFor(p => p.UpdatedAt, f => f.Random.Bool() ? f.Date.Recent() : null);

    /// <summary>
    /// Configures the Faker to generate valid ProductDto entities.
    /// </summary>
    private static readonly Faker<ProductDto> productDtoFaker = new Faker<ProductDto>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 9999))
        .RuleFor(p => p.IsActive, f => f.Random.Bool())
        .RuleFor(p => p.CreatedAt, f => f.Date.Recent());

    /// <summary>
    /// Generates a valid CreateProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateProductCommand with randomly generated data.</returns>
    public static CreateProductCommand GenerateValidCreateCommand()
    {
        return createProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetProductQuery with randomized data.
    /// </summary>
    /// <returns>A valid GetProductQuery with randomly generated data.</returns>
    public static GetProductQuery GenerateValidGetQuery()
    {
        return getProductQueryFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetProductsQuery with randomized data.
    /// </summary>
    /// <returns>A valid GetProductsQuery with randomly generated data.</returns>
    public static GetProductsQuery GenerateValidGetProductsQuery()
    {
        return getProductsQueryFaker.Generate();
    }

    /// <summary>
    /// Generates a valid UpdateProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid UpdateProductCommand with randomly generated data.</returns>
    public static UpdateProductCommand GenerateValidUpdateCommand()
    {
        return updateProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid DeleteProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid DeleteProductCommand with randomly generated data.</returns>
    public static DeleteProductCommand GenerateValidDeleteCommand()
    {
        return deleteProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return productFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Product entities with randomized data.
    /// </summary>
    /// <param name="count">Number of products to generate</param>
    /// <returns>A list of valid Product entities with randomly generated data.</returns>
    public static List<Product> GenerateValidProducts(int count = 5)
    {
        return productFaker.Generate(count);
    }

    /// <summary>
    /// Generates a valid ProductDto with randomized data.
    /// </summary>
    /// <returns>A valid ProductDto with randomly generated data.</returns>
    public static ProductDto GenerateValidProductDto()
    {
        return productDtoFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid ProductDto entities with randomized data.
    /// </summary>
    /// <param name="count">Number of product DTOs to generate</param>
    /// <returns>A list of valid ProductDto entities with randomly generated data.</returns>
    public static List<ProductDto> GenerateValidProductDtos(int count = 5)
    {
        return productDtoFaker.Generate(count);
    }
} 