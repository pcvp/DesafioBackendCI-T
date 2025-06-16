using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetProductsHandler"/> class.
/// </summary>
public class GetProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductsHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductsHandler(_productRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid products retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid query When getting products Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var query = ProductTestData.GenerateValidGetProductsQuery();
        var products = ProductTestData.GenerateValidProducts(3);
        var productDtos = ProductTestData.GenerateValidProductDtos(3);
        
        _productRepository.GetPagedAsync(query.Page, query.Size, query.Search, Arg.Any<CancellationToken>())
            .Returns((products, products.Count));
        _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.CurrentPage.Should().Be(query.Page);
        result.TotalCount.Should().Be(products.Count);
        result.TotalPages.Should().BeGreaterThanOrEqualTo(1);
    }

    /// <summary>
    /// Tests that an invalid products retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid query When getting products Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var query = new GetProductsQuery
        {
            Page = 0, // Invalid page
            Size = 10
        };

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that search functionality works correctly.
    /// </summary>
    [Fact(DisplayName = "Given query with search term When getting products Then filters results")]
    public async Task Handle_WithSearchTerm_FiltersResults()
    {
        // Given
        var query = new GetProductsQuery
        {
            Page = 1,
            Size = 10,
            Search = "Dell"
        };
        var products = new List<Product>();
        var productDtos = new List<ProductDto>();
        
        _productRepository.GetPagedAsync(query.Page, query.Size, query.Search, Arg.Any<CancellationToken>())
            .Returns((products, products.Count));
        _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        await _productRepository.Received(1).GetPagedAsync(query.Page, query.Size, "Dell", Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that pagination works correctly.
    /// </summary>
    [Fact(DisplayName = "Given query with pagination When getting products Then returns correct page")]
    public async Task Handle_WithPagination_ReturnsCorrectPage()
    {
        // Given
        var query = new GetProductsQuery
        {
            Page = 1,
            Size = 2
        };
        var products = ProductTestData.GenerateValidProducts(2);
        var productDtos = ProductTestData.GenerateValidProductDtos(2);
        
        _productRepository.GetPagedAsync(query.Page, query.Size, query.Search, Arg.Any<CancellationToken>())
            .Returns((products, products.Count));
        _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(1);
        result.Products.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests that validation is performed for invalid page size.
    /// </summary>
    [Fact(DisplayName = "Given query with invalid size When handling Then throws validation exception")]
    public async Task Handle_InvalidSize_ThrowsValidationException()
    {
        // Given
        var query = new GetProductsQuery
        {
            Page = 1,
            Size = 0 // Invalid size
        };

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(GetProductsQuery.Size)));
    }

    /// <summary>
    /// Tests that HasNext and HasPrevious flags are set correctly.
    /// </summary>
    [Fact(DisplayName = "Given query for first page When handling Then sets pagination flags correctly")]
    public async Task Handle_FirstPage_SetsPaginationFlagsCorrectly()
    {
        // Given
        var query = new GetProductsQuery
        {
            Page = 1,
            Size = 2
        };
        var products = ProductTestData.GenerateValidProducts(5);
        var productDtos = ProductTestData.GenerateValidProductDtos(2);
        
        _productRepository.GetPagedAsync(query.Page, query.Size, query.Search, Arg.Any<CancellationToken>())
            .Returns((products.Take(2), 5));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.HasPrevious.Should().BeFalse();
        result.HasNext.Should().BeTrue();
        result.TotalCount.Should().Be(5);
        result.TotalPages.Should().Be(3);
    }
} 