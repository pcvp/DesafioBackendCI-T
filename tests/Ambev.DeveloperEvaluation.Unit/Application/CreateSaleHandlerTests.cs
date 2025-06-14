using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SaleTestData.GenerateValidCreateCommand();
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId,
            command.ProductId,
            command.Quantity,
            command.UnitPrice,
            command.Discount)
        {
            Id = Guid.NewGuid()
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            ProductId = sale.ProductId,
            Quantity = sale.Quantity,
            UnitPrice = sale.UnitPrice,
            Discount = sale.Discount,
            TotalAmount = sale.TotalAmount,
            TotalSaleAmount = sale.TotalSaleAmount,
            IsCancelled = sale.IsCancelled,
            CreatedAt = sale.CreatedAt
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        createSaleResult.SaleNumber.Should().Be(command.SaleNumber);
        createSaleResult.TotalAmount.Should().Be(sale.TotalAmount);
        createSaleResult.IsCancelled.Should().BeFalse();
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = SaleTestData.GenerateInvalidCreateCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that duplicate sale number throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given duplicate sale number When creating sale Then throws validation exception")]
    public async Task Handle_DuplicateSaleNumber_ThrowsValidationException()
    {
        // Given
        var command = SaleTestData.GenerateValidCreateCommand();
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.UpdateSaleInfo(command.SaleNumber, command.SaleDate, command.CustomerId, command.BranchId);

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateSaleCommand.SaleNumber)));
    }

    /// <summary>
    /// Tests that the sale is created with correct properties.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates sale with correct properties")]
    public async Task Handle_ValidRequest_CreatesSaleWithCorrectProperties()
    {
        // Given
        var command = SaleTestData.GenerateValidCreateCommand();
        var sale = SaleTestData.GenerateValidSale();

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.SaleNumber == command.SaleNumber &&
                s.SaleDate == command.SaleDate &&
                s.CustomerId == command.CustomerId &&
                s.BranchId == command.BranchId &&
                s.ProductId == command.ProductId &&
                s.Quantity == command.Quantity &&
                s.UnitPrice == command.UnitPrice &&
                s.Discount == command.Discount &&
                s.IsCancelled == false),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the created sale.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps sale to result")]
    public async Task Handle_ValidRequest_MapsSaleToResult()
    {
        // Given
        var command = SaleTestData.GenerateValidCreateCommand();
        var sale = SaleTestData.GenerateValidSale();

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<CreateSaleResult>(Arg.Is<Sale>(s =>
            s.SaleNumber == command.SaleNumber &&
            s.CustomerId == command.CustomerId &&
            s.BranchId == command.BranchId));
    }

    /// <summary>
    /// Tests that validation is performed before creating the sale.
    /// </summary>
    [Fact(DisplayName = "Given command with empty sale number When handling Then throws validation exception")]
    public async Task Handle_EmptySaleNumber_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = string.Empty,
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            UnitPrice = 10.50m,
            Discount = 0,
            TotalAmount = 10.50m,
            TotalSaleAmount = 10.50m
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateSaleCommand.SaleNumber)));
    }

    /// <summary>
    /// Tests that validation is performed for invalid quantity.
    /// </summary>
    [Fact(DisplayName = "Given command with zero quantity When handling Then throws validation exception")]
    public async Task Handle_ZeroQuantity_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 0,
            UnitPrice = 10.50m,
            Discount = 0,
            TotalAmount = 0,
            TotalSaleAmount = 0
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateSaleCommand.Quantity)));
    }

    /// <summary>
    /// Tests that validation is performed for negative unit price.
    /// </summary>
    [Fact(DisplayName = "Given command with negative unit price When handling Then throws validation exception")]
    public async Task Handle_NegativeUnitPrice_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            UnitPrice = -10.50m,
            Discount = 0,
            TotalAmount = -10.50m,
            TotalSaleAmount = -10.50m
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateSaleCommand.UnitPrice)));
    }

    /// <summary>
    /// Tests that total amount is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When creating sale Then calculates total amount correctly")]
    public async Task Handle_ValidCommand_CalculatesTotalAmountCorrectly()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 2,
            UnitPrice = 100m,
            Discount = 10m, // 10%
            TotalAmount = 180m, // 2 * 100 * (1 - 0.1) = 180
            TotalSaleAmount = 180m
        };

        var sale = SaleTestData.GenerateValidSale();

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => s.TotalAmount == 180m),
            Arg.Any<CancellationToken>());
    }
} 