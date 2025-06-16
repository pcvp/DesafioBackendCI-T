using Xunit;
using FluentAssertions;
using NSubstitute;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Example demonstrating how events work with the LoggingMessagePublisher
/// </summary>
public class EventIntegrationExample
{
    [Fact]
    public async Task LoggingMessagePublisher_Should_Log_SaleCreatedEvent()
    {
        // Arrange
        var logger = Substitute.For<ILogger<LoggingMessagePublisher>>();
        var publisher = new LoggingMessagePublisher(logger);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var sale = new Sale("SALE001", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        var saleCreatedEvent = mapper.Map<SaleCreatedEvent>(sale);

        // Act
        await publisher.PublishAsync(EventTopics.SaleCreated, saleCreatedEvent);

        // Assert
        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("MESSAGE PUBLISHED to topic 'sale.created'")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Fact]
    public async Task LoggingMessagePublisher_Should_Log_SaleModifiedEvent()
    {
        // Arrange
        var logger = Substitute.For<ILogger<LoggingMessagePublisher>>();
        var publisher = new LoggingMessagePublisher(logger);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var sale = new Sale("SALE002", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        sale.UpdateSaleInfo("SALE002-UPDATED", DateTime.UtcNow, sale.CustomerId, sale.BranchId);
        
        var saleModifiedEvent = mapper.Map<SaleModifiedEvent>(sale);

        // Act
        await publisher.PublishAsync(EventTopics.SaleModified, saleModifiedEvent);

        // Assert
        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("MESSAGE PUBLISHED to topic 'sale.modified'")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Fact]
    public async Task LoggingMessagePublisher_Should_Log_SaleCancelledEvent()
    {
        // Arrange
        var logger = Substitute.For<ILogger<LoggingMessagePublisher>>();
        var publisher = new LoggingMessagePublisher(logger);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var sale = new Sale("SALE003", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        sale.Cancel();
        
        var saleCancelledEvent = mapper.Map<SaleCancelledEvent>(sale);

        // Act
        await publisher.PublishAsync(EventTopics.SaleCancelled, saleCancelledEvent);

        // Assert
        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("MESSAGE PUBLISHED to topic 'sale.cancelled'")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Fact]
    public async Task LoggingMessagePublisher_Should_Log_SaleItemCancelledEvent()
    {
        // Arrange
        var logger = Substitute.For<ILogger<LoggingMessagePublisher>>();
        var publisher = new LoggingMessagePublisher(logger);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 3, 15.75m, 0);
        var saleItemCancelledEvent = mapper.Map<SaleItemCancelledEvent>(saleItem);

        // Act
        await publisher.PublishAsync(EventTopics.SaleItemCancelled, saleItemCancelledEvent);

        // Assert
        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("MESSAGE PUBLISHED to topic 'sale.item.cancelled'")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Fact]
    public void EventMapping_Should_Preserve_All_Important_Data()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sale = new Sale("SALE-COMPLETE", DateTime.UtcNow, customerId, branchId);
        
        // Add an item to the sale
        var productId = Guid.NewGuid();
        var saleItem = new SaleItem(sale.Id, productId, 5, 20.00m, 10m);
        sale.AddItem(saleItem);

        // Act - Map to different event types
        var createdEvent = mapper.Map<SaleCreatedEvent>(sale);
        var modifiedEvent = mapper.Map<SaleModifiedEvent>(sale);
        var cancelledEvent = mapper.Map<SaleCancelledEvent>(sale);
        var itemCancelledEvent = mapper.Map<SaleItemCancelledEvent>(saleItem);

        // Assert - All events should have correct data
        createdEvent.SaleId.Should().Be(sale.Id);
        createdEvent.SaleNumber.Should().Be("SALE-COMPLETE");
        createdEvent.CustomerId.Should().Be(customerId);
        createdEvent.BranchId.Should().Be(branchId);

        modifiedEvent.SaleId.Should().Be(sale.Id);
        modifiedEvent.TotalAmount.Should().Be(sale.TotalAmount);

        cancelledEvent.SaleId.Should().Be(sale.Id);
        cancelledEvent.TotalAmount.Should().Be(sale.TotalAmount);

        itemCancelledEvent.SaleItemId.Should().Be(saleItem.Id);
        itemCancelledEvent.ProductId.Should().Be(productId);
        itemCancelledEvent.Quantity.Should().Be(5);
        itemCancelledEvent.UnitPrice.Should().Be(20.00m);
        itemCancelledEvent.TotalAmount.Should().Be(saleItem.TotalAmount);
    }
} 