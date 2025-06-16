using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid get sale request returns the sale successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When getting sale Then returns sale")]
    public async Task Handle_ValidRequest_ReturnsSale()
    {
        // Given
        var query = SaleTestData.GenerateValidGetQuery();
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = query.Id;

        var result = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            Status = sale.Status,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt
        };

        _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(query, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Id.Should().Be(sale.Id);
        getSaleResult.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).GetByIdAsync(query.Id, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when sale is not found, throws exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale id When getting sale Then throws exception")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Given
        var query = SaleTestData.GenerateValidGetQuery();

        _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {query.Id} not found");
        await _saleRepository.Received(1).GetByIdAsync(query.Id, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetSaleResult>(Arg.Any<Sale>());
    }

    /// <summary>
    /// Tests that validation is performed on the query.
    /// </summary>
    [Fact(DisplayName = "Given invalid query When getting sale Then throws validation exception")]
    public async Task Handle_InvalidQuery_ThrowsValidationException()
    {
        // Given
        var query = new GetSaleQuery { Id = Guid.Empty };

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Sale ID cannot be empty*");
    }

    /// <summary>
    /// Tests that the mapper is called with the found sale.
    /// </summary>
    [Fact(DisplayName = "Given valid query When sale found Then maps sale to result")]
    public async Task Handle_SaleFound_MapsSaleToResult()
    {
        // Given
        var query = SaleTestData.GenerateValidGetQuery();
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = query.Id;

        _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(new GetSaleResult());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetSaleResult>(Arg.Is<Sale>(s => s.Id == query.Id));
    }

    /// <summary>
    /// Tests that cancelled sales are returned correctly.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When getting sale Then returns cancelled sale")]
    public async Task Handle_CancelledSale_ReturnsCancelledSale()
    {
        // Given
        var query = SaleTestData.GenerateValidGetQuery();
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = query.Id;
        sale.Cancel();

        var result = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            Status = sale.Status,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt
        };

        _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(query, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Status.Should().Be(sale.Status);
    }
} 