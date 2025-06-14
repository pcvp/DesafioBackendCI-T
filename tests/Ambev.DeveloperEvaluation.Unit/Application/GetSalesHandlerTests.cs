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
    /// Tests that a valid get sales request returns paginated sales successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid query When getting sales Then returns paginated sales")]
    public async Task Handle_ValidRequest_ReturnsPaginatedSales()
    {
        // Given
        var query = SaleTestData.GenerateValidGetSalesQuery();
        var sales = SaleTestData.GenerateValidSales(3);
        var totalCount = 10;

        var saleResults = sales.Select(s => new SaleDto
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerId = s.CustomerId,
            BranchId = s.BranchId,
            ProductId = s.ProductId,
            Quantity = s.Quantity,
            UnitPrice = s.UnitPrice,
            Discount = s.Discount,
            TotalAmount = s.TotalAmount,
            TotalSaleAmount = s.TotalSaleAmount,
            IsCancelled = s.IsCancelled,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        _saleRepository.GetPagedAsync(
            query.Page,
            query.Size,
            query.CustomerId,
            query.BranchId,
            query.StartDate,
            query.EndDate,
            query.IsCancelled,
            Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleDto>>(sales).Returns(saleResults);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(3);
        result.TotalCount.Should().Be(totalCount);
        result.Page.Should().Be(query.Page);
        result.Size.Should().Be(query.Size);
        result.TotalPages.Should().Be((int)Math.Ceiling((double)totalCount / query.Size));
        await _saleRepository.Received(1).GetPagedAsync(
            query.Page,
            query.Size,
            query.CustomerId,
            query.BranchId,
            query.StartDate,
            query.EndDate,
            query.IsCancelled,
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when no sales are found, returns empty result.
    /// </summary>
    [Fact(DisplayName = "Given query with no results When getting sales Then returns empty result")]
    public async Task Handle_NoSalesFound_ReturnsEmptyResult()
    {
        // Given
        var query = SaleTestData.GenerateValidGetSalesQuery();
        var emptySales = new List<Sale>();
        var totalCount = 0;

        _saleRepository.GetPagedAsync(
            query.Page,
            query.Size,
            query.CustomerId,
            query.BranchId,
            query.StartDate,
            query.EndDate,
            query.IsCancelled,
            Arg.Any<CancellationToken>())
            .Returns((emptySales, totalCount));

        _mapper.Map<List<SaleDto>>(emptySales).Returns(new List<SaleDto>());

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    /// <summary>
    /// Tests that validation is performed on the query.
    /// </summary>
    [Fact(DisplayName = "Given invalid query When getting sales Then throws validation exception")]
    public async Task Handle_InvalidQuery_ThrowsValidationException()
    {
        // Given
        var query = new GetSalesQuery
        {
            Page = 0, // Invalid: page must be >= 1
            Size = 10
        };

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(GetSalesQuery.Page)));
    }

    /// <summary>
    /// Tests that filtering by customer works correctly.
    /// </summary>
    [Fact(DisplayName = "Given query with customer filter When getting sales Then filters by customer")]
    public async Task Handle_QueryWithCustomerFilter_FiltersCorrectly()
    {
        // Given
        var customerId = Guid.NewGuid();
        var query = new GetSalesQuery
        {
            Page = 1,
            Size = 10,
            CustomerId = customerId
        };

        var sales = SaleTestData.GenerateValidSales(2);
        var totalCount = 2;

        _saleRepository.GetPagedAsync(
            query.Page,
            query.Size,
            customerId,
            null,
            null,
            null,
            null,
            Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleDto>>(sales).Returns(new List<SaleDto>());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetPagedAsync(
            query.Page,
            query.Size,
            customerId,
            null,
            null,
            null,
            null,
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that filtering by date range works correctly.
    /// </summary>
    [Fact(DisplayName = "Given query with date range When getting sales Then filters by date range")]
    public async Task Handle_QueryWithDateRange_FiltersCorrectly()
    {
        // Given
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;
        var query = new GetSalesQuery
        {
            Page = 1,
            Size = 10,
            StartDate = startDate,
            EndDate = endDate
        };

        var sales = SaleTestData.GenerateValidSales(2);
        var totalCount = 2;

        _saleRepository.GetPagedAsync(
            query.Page,
            query.Size,
            null,
            null,
            startDate,
            endDate,
            null,
            Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleDto>>(sales).Returns(new List<SaleDto>());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetPagedAsync(
            query.Page,
            query.Size,
            null,
            null,
            startDate,
            endDate,
            null,
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that filtering by cancelled status works correctly.
    /// </summary>
    [Fact(DisplayName = "Given query with cancelled filter When getting sales Then filters by cancelled status")]
    public async Task Handle_QueryWithCancelledFilter_FiltersCorrectly()
    {
        // Given
        var query = new GetSalesQuery
        {
            Page = 1,
            Size = 10,
            IsCancelled = true
        };

        var sales = SaleTestData.GenerateValidSales(2);
        var totalCount = 2;

        _saleRepository.GetPagedAsync(
            query.Page,
            query.Size,
            null,
            null,
            null,
            null,
            true,
            Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleDto>>(sales).Returns(new List<SaleDto>());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetPagedAsync(
            query.Page,
            query.Size,
            null,
            null,
            null,
            null,
            true,
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the found sales.
    /// </summary>
    [Fact(DisplayName = "Given valid query When sales found Then maps sales to DTOs")]
    public async Task Handle_SalesFound_MapsSalesToDtos()
    {
        // Given
        var query = SaleTestData.GenerateValidGetSalesQuery();
        var sales = SaleTestData.GenerateValidSales(2);
        var totalCount = 2;

        _saleRepository.GetPagedAsync(
            Arg.Any<int>(),
            Arg.Any<int>(),
            Arg.Any<Guid?>(),
            Arg.Any<Guid?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<bool?>(),
            Arg.Any<CancellationToken>())
            .Returns((sales, totalCount));

        _mapper.Map<List<SaleDto>>(sales).Returns(new List<SaleDto>());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<List<SaleDto>>(Arg.Is<List<Sale>>(s => s.Count == 2));
    }
} 