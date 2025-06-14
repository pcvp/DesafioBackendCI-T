using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid update sale request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid update data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;

        var result = new UpdateSaleResult
        {
            Id = existingSale.Id,
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            CustomerId = command.CustomerId,
            BranchId = command.BranchId,
            ProductId = command.ProductId,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            Discount = command.Discount,
            TotalAmount = command.TotalAmount,
            TotalSaleAmount = command.TotalSaleAmount,
            IsCancelled = command.IsCancelled,
            UpdatedAt = DateTime.UtcNow
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(result);

        // When
        var updateResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(command.Id);
        updateResult.SaleNumber.Should().Be(command.SaleNumber);
        updateResult.TotalAmount.Should().Be(command.TotalAmount);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that updating non-existent sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When updating Then throws exception")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale not found");
    }

    /// <summary>
    /// Tests that duplicate sale number throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given duplicate sale number When updating sale Then throws validation exception")]
    public async Task Handle_DuplicateSaleNumber_ThrowsValidationException()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;

        var duplicateSale = SaleTestData.GenerateValidSale();
        duplicateSale.Id = Guid.NewGuid(); // Different ID
        duplicateSale.UpdateSaleInfo(command.SaleNumber, DateTime.Now, Guid.NewGuid(), Guid.NewGuid());

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(duplicateSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateSaleCommand.SaleNumber)));
    }

    /// <summary>
    /// Tests that invalid update request throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid update data When updating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = SaleTestData.GenerateInvalidUpdateCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that cancelling a sale works correctly.
    /// </summary>
    [Fact(DisplayName = "Given active sale When cancelling Then sale is cancelled")]
    public async Task Handle_CancellingSale_SaleIsCancelled()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        command.IsCancelled = true;

        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        existingSale.Reactivate(); // Ensure it's active

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.IsCancelled == true),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that reactivating a cancelled sale works correctly.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When reactivating Then sale is reactivated")]
    public async Task Handle_ReactivatingSale_SaleIsReactivated()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        command.IsCancelled = false;

        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        existingSale.Cancel(); // Ensure it's cancelled

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.IsCancelled == false),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale information is updated correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid update command When updating Then updates sale information")]
    public async Task Handle_ValidUpdate_UpdatesSaleInformation()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => 
                s.SaleNumber == command.SaleNumber &&
                s.SaleDate == command.SaleDate &&
                s.CustomerId == command.CustomerId &&
                s.BranchId == command.BranchId &&
                s.ProductId == command.ProductId &&
                s.Quantity == command.Quantity &&
                s.UnitPrice == command.UnitPrice &&
                s.Discount == command.Discount),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the updated sale.
    /// </summary>
    [Fact(DisplayName = "Given valid update When handling Then maps sale to result")]
    public async Task Handle_ValidUpdate_MapsSaleToResult()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<UpdateSaleResult>(Arg.Is<Sale>(s => s.Id == command.Id));
    }

    /// <summary>
    /// Tests that validation is performed for empty sale number.
    /// </summary>
    [Fact(DisplayName = "Given empty sale number When updating Then throws validation exception")]
    public async Task Handle_EmptySaleNumber_ThrowsValidationException()
    {
        // Given
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleNumber = string.Empty,
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            UnitPrice = 10.50m,
            Discount = 0,
            TotalAmount = 10.50m,
            TotalSaleAmount = 10.50m,
            IsCancelled = false
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateSaleCommand.SaleNumber)));
    }
} 