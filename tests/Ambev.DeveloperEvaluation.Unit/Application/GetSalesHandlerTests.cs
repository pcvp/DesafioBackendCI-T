using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetSalesHandler"/> class.
/// </summary>
public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSalesHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSalesHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid query returns sales successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should return sales when query is valid")]
    public async Task Handle_ValidQuery_ReturnsSuccess()
    {
        // Given
        var query = new GetSalesQuery { Page = 1, Size = 10 };
        var sales = SaleTestData.GenerateValidSalesList(2);
        var totalCount = 10;

        var saleResults = sales.Select(s => new SaleResultDto
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerId = s.CustomerId,
            BranchId = s.BranchId,
            Status = s.Status,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        var expectedResult = new GetSalesResult
        {
            Sales = saleResults,
            CurrentPage = query.Page,
            TotalPages = (int)Math.Ceiling((double)totalCount / query.Size),
            TotalCount = totalCount,
            HasNext = query.Page < (int)Math.Ceiling((double)totalCount / query.Size),
            HasPrevious = query.Page > 1
        };

        _saleRepository.GetPagedAsync(query.Page, query.Size, query.Search, null, null, null, Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleResultDto>>(sales).Returns(saleResults);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(2);
        result.CurrentPage.Should().Be(query.Page);
        result.TotalCount.Should().Be(totalCount);
        await _saleRepository.Received(1).GetPagedAsync(query.Page, query.Size, query.Search, null, null, null, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<SaleResultDto>>(Arg.Is<List<Sale>>(s => s.Count == 2));
    }

    /// <summary>
    /// Tests that empty results are handled correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should return empty result when no sales found")]
    public async Task Handle_NoSalesFound_ReturnsEmptyResult()
    {
        // Given
        var query = new GetSalesQuery { Page = 1, Size = 10 };
        var emptySales = new List<Sale>();
        var totalCount = 0;

        _saleRepository.GetPagedAsync(query.Page, query.Size, query.Search, null, null, null, Arg.Any<CancellationToken>())
            .Returns((emptySales, totalCount));

        _mapper.Map<List<SaleResultDto>>(emptySales).Returns(new List<SaleResultDto>());

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().BeEmpty();
        result.CurrentPage.Should().Be(query.Page);
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.HasNext.Should().BeFalse();
        result.HasPrevious.Should().BeFalse();
    }

    /// <summary>
    /// Tests that pagination works correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should handle pagination correctly")]
    public async Task Handle_WithPagination_ReturnsPaginatedResult()
    {
        // Given
        var query = new GetSalesQuery { Page = 2, Size = 5 };
        var sales = SaleTestData.GenerateValidSalesList(5);
        var totalCount = 15;

        _saleRepository.GetPagedAsync(query.Page, query.Size, query.Search, null, null, null, Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleResultDto>>(sales).Returns(new List<SaleResultDto>());

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(2);
        result.TotalPages.Should().Be(3);
        result.TotalCount.Should().Be(15);
        result.HasNext.Should().BeTrue();
        result.HasPrevious.Should().BeTrue();
    }

    /// <summary>
    /// Tests that search functionality works correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should filter sales by search term")]
    public async Task Handle_WithSearchTerm_ReturnsFilteredResults()
    {
        // Given
        var query = new GetSalesQuery { Page = 1, Size = 10, Search = "SALE001" };
        var sales = SaleTestData.GenerateValidSalesList(1);
        var totalCount = 1;

        var saleResults = sales.Select(s => new SaleResultDto
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerId = s.CustomerId,
            BranchId = s.BranchId,
            Status = s.Status,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        _saleRepository.GetPagedAsync(query.Page, query.Size, query.Search, null, null, null, Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleResultDto>>(sales).Returns(saleResults);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        await _saleRepository.Received(1).GetPagedAsync(query.Page, query.Size, "SALE001", null, null, null, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should call mapper with correct parameters")]
    public async Task Handle_ValidQuery_CallsMapperCorrectly()
    {
        // Given
        var query = new GetSalesQuery { Page = 1, Size = 10 };
        var sales = SaleTestData.GenerateValidSalesList(2);
        var totalCount = 2;

        _saleRepository.GetPagedAsync(query.Page, query.Size, query.Search, null, null, null, Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleResultDto>>(sales).Returns(new List<SaleResultDto>());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<List<SaleResultDto>>(Arg.Is<List<Sale>>(s => s.Count == 2));
    }
} 