using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid product retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product ID When getting product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var query = ProductTestData.GenerateValidGetQuery();
        var product = ProductTestData.GenerateValidProduct();
        product.Id = query.Id;

        var result = new GetProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };

        _productRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(result);

        // When
        var getProductResult = await _handler.Handle(query, CancellationToken.None);

        // Then
        getProductResult.Should().NotBeNull();
        getProductResult.Id.Should().Be(product.Id);
        getProductResult.Name.Should().Be(product.Name);
        getProductResult.Price.Should().Be(product.Price);
        getProductResult.IsActive.Should().Be(product.IsActive);
        await _productRepository.Received(1).GetByIdAsync(query.Id, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid product retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product ID When getting product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var query = new GetProductQuery(); // Empty query will fail validation

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that when product is not found, a KeyNotFoundException is thrown.
    /// </summary>
    [Fact(DisplayName = "Given non-existent product ID When getting product Then throws KeyNotFoundException")]
    public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
    {
        // Given
        var query = ProductTestData.GenerateValidGetQuery();

        _productRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {query.Id} not found");
    }

    /// <summary>
    /// Tests that the mapper is called with the retrieved product.
    /// </summary>
    [Fact(DisplayName = "Given valid query When handling Then maps product to result")]
    public async Task Handle_ValidRequest_MapsProductToResult()
    {
        // Given
        var query = ProductTestData.GenerateValidGetQuery();
        var product = ProductTestData.GenerateValidProduct();
        product.Id = query.Id;

        _productRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(new GetProductResult());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetProductResult>(Arg.Is<Product>(p =>
            p.Id == query.Id &&
            p.Name == product.Name &&
            p.Price == product.Price));
    }

    /// <summary>
    /// Tests that validation is performed for empty GUID.
    /// </summary>
    [Fact(DisplayName = "Given empty GUID When handling Then throws validation exception")]
    public async Task Handle_EmptyGuid_ThrowsValidationException()
    {
        // Given
        var query = new GetProductQuery
        {
            Id = Guid.Empty
        };

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(GetProductQuery.Id)));
    }
} 