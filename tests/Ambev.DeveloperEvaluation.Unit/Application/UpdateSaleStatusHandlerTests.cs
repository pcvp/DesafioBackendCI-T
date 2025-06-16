using Xunit;
using FluentAssertions;
using NSubstitute;
using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleStatus;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleStatusHandler"/> class.
/// </summary>
public class UpdateSaleStatusHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;
    private readonly UpdateSaleStatusHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleStatusHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleStatusHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new UpdateSaleStatusHandler(_saleRepository, _unitOfWork, _mapper, _messagePublisher);
    }

    /// <summary>
    /// Tests that closing a sale with 4-9 identical items applies 10% discount automatically.
    /// </summary>
    [Fact(DisplayName = "Handle should apply 10% discount when closing sale with 4-9 identical items")]
    public async Task Given_SaleWith4To9IdenticalItems_When_ClosingSale_Then_ShouldApply10PercentDiscount()
    {
        // Arrange
        var command = new UpdateSaleStatusCommand
        {
            Id = Guid.NewGuid(),
            Status = SaleStatusEnum.Closed
        };

        var sale = CreateSaleWithItems(command.Id, productId: Guid.NewGuid(), quantity: 5); // 5 identical items
        
        _saleRepository.GetByIdWithItemsAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Status.Should().Be(SaleStatusEnum.Closed);
        sale.Items.First().Discount.Should().Be(10m); // 10% discount applied
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that closing a sale with 10-20 identical items applies 20% discount automatically.
    /// </summary>
    [Fact(DisplayName = "Handle should apply 20% discount when closing sale with 10-20 identical items")]
    public async Task Given_SaleWith10To20IdenticalItems_When_ClosingSale_Then_ShouldApply20PercentDiscount()
    {
        // Arrange
        var command = new UpdateSaleStatusCommand
        {
            Id = Guid.NewGuid(),
            Status = SaleStatusEnum.Closed
        };

        var sale = CreateSaleWithItems(command.Id, productId: Guid.NewGuid(), quantity: 15); // 15 identical items
        
        _saleRepository.GetByIdWithItemsAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Status.Should().Be(SaleStatusEnum.Closed);
        sale.Items.First().Discount.Should().Be(20m); // 20% discount applied
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that closing a sale with less than 4 identical items removes any existing discount.
    /// </summary>
    [Fact(DisplayName = "Handle should remove discount when closing sale with less than 4 identical items")]
    public async Task Given_SaleWithLessThan4IdenticalItems_When_ClosingSale_Then_ShouldRemoveDiscount()
    {
        // Arrange
        var command = new UpdateSaleStatusCommand
        {
            Id = Guid.NewGuid(),
            Status = SaleStatusEnum.Closed
        };

        var sale = CreateSaleWithItems(command.Id, productId: Guid.NewGuid(), quantity: 3, initialDiscount: 15m); // 3 items with existing discount
        
        _saleRepository.GetByIdWithItemsAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Status.Should().Be(SaleStatusEnum.Closed);
        sale.Items.First().Discount.Should().Be(0m); // Discount removed
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that closing a sale with more than 20 identical items throws exception.
    /// </summary>
    [Fact(DisplayName = "Handle should throw exception when closing sale with more than 20 identical items")]
    public async Task Given_SaleWithMoreThan20IdenticalItems_When_ClosingSale_Then_ShouldThrowException()
    {
        // Arrange
        var command = new UpdateSaleStatusCommand
        {
            Id = Guid.NewGuid(),
            Status = SaleStatusEnum.Closed
        };

        var sale = CreateSaleWithItems(command.Id, productId: Guid.NewGuid(), quantity: 25); // 25 identical items
        
        _saleRepository.GetByIdWithItemsAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Contain("Cannot sell more than 20 identical items");
        sale.Status.Should().Be(SaleStatusEnum.Pending); // Status should not change
    }

    /// <summary>
    /// Helper method to create a sale with items for testing.
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="initialDiscount">Initial discount percentage</param>
    /// <returns>A sale with the specified items</returns>
    private static Sale CreateSaleWithItems(Guid saleId, Guid productId, int quantity, decimal initialDiscount = 0m)
    {
        var sale = new Sale("SALE001", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        sale.Id = saleId;

        var item = new SaleItem(saleId, productId, quantity, 100m, initialDiscount);
        sale.AddItem(item);

        return sale;
    }
}
