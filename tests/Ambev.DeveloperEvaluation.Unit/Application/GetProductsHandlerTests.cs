using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
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
    private readonly IMapper _mapper;
    private readonly GetProductsHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetProductsHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductsHandler(_mapper);
    }

    /// <summary>
    /// Tests that a valid products retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid query When getting products Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var query = ProductTestData.GenerateValidGetProductsQuery();

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.CurrentPage.Should().Be(query.Page);
        result.TotalCount.Should().BeGreaterThanOrEqualTo(0);
        result.TotalPages.Should().BeGreaterThanOrEqualTo(0);
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

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        if (result.Products.Any())
        {
            result.Products.Should().OnlyContain(p => p.Name.Contains("Dell", StringComparison.OrdinalIgnoreCase));
        }
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

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(1);
        result.Products.Should().HaveCountLessOrEqualTo(2);
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

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.HasPrevious.Should().BeFalse();
        if (result.TotalCount > result.Products.Count)
        {
            result.HasNext.Should().BeTrue();
        }
    }
} 