using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _unitOfWork, _messagePublisher);
    }

    /// <summary>
    /// Tests that a valid update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid update data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        command.Status = SaleStatusEnum.Pending; // Ensure it's pending so it can be updated
        
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        // Ensure the sale is pending (not cancelled)
        if (existingSale.Status == SaleStatusEnum.Cancelled)
        {
            existingSale.Reactivate();
        }

        var expectedResult = new UpdateSaleResult
        {
            Id = existingSale.Id,
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            CustomerId = command.CustomerId,
            BranchId = command.BranchId,
            Status = existingSale.Status,
            CreatedAt = existingSale.CreatedAt,
            UpdatedAt = existingSale.UpdatedAt
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(expectedResult);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var updateResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(command.Id);
        updateResult.SaleNumber.Should().Be(command.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that non-existent sale throws exception.
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
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
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
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with number {command.SaleNumber} already exists");
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
        command.Status = Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Cancelled; // Set to cancel the sale

        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        // Sale is already active by default, no need to call Reactivate()

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.Status == Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Cancelled),
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
        command.Status = Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Pending; // Set to reactivate the sale

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
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.Status == Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Pending),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a valid update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid update command When updating Then updates sale information")]
    public async Task Handle_ValidUpdate_UpdatesSaleInformation()
    {
        // Given
        var command = SaleTestData.GenerateValidUpdateCommand();
        command.Status = SaleStatusEnum.Pending; // Ensure it's pending so it can be updated
        
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        // Ensure the sale is pending (not cancelled)
        if (existingSale.Status == SaleStatusEnum.Cancelled)
        {
            existingSale.Reactivate();
        }

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.SaleNumber == command.SaleNumber &&
                             s.SaleDate == command.SaleDate &&
                             s.CustomerId == command.CustomerId &&
                             s.BranchId == command.BranchId),
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
        command.Status = SaleStatusEnum.Pending; // Ensure it's pending so it can be updated
        
        var existingSale = SaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        // Ensure the sale is pending (not cancelled)
        if (existingSale.Status == SaleStatusEnum.Cancelled)
        {
            existingSale.Reactivate();
        }

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(new UpdateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<UpdateSaleResult>(existingSale);
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
            BranchId = Guid.NewGuid()
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateSaleCommand.SaleNumber)));
    }
} 