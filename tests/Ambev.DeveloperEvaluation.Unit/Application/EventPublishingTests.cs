using Xunit;
using FluentAssertions;
using NSubstitute;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Application.Events;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Tests to demonstrate event publishing functionality
/// </summary>
public class EventPublishingTests
{
    [Fact]
    public void EventMappingProfile_Should_Map_Sale_To_SaleCreatedEvent()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var sale = new Sale("SALE001", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());

        // Act
        var saleCreatedEvent = mapper.Map<SaleCreatedEvent>(sale);

        // Assert
        saleCreatedEvent.Should().NotBeNull();
        saleCreatedEvent.SaleId.Should().Be(sale.Id);
        saleCreatedEvent.SaleNumber.Should().Be(sale.SaleNumber);
        saleCreatedEvent.CustomerId.Should().Be(sale.CustomerId);
        saleCreatedEvent.BranchId.Should().Be(sale.BranchId);
        saleCreatedEvent.Status.Should().Be(sale.Status);
        saleCreatedEvent.EventTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EventMappingProfile_Should_Map_Sale_To_SaleModifiedEvent()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var sale = new Sale("SALE002", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());

        // Act
        var saleModifiedEvent = mapper.Map<SaleModifiedEvent>(sale);

        // Assert
        saleModifiedEvent.Should().NotBeNull();
        saleModifiedEvent.SaleId.Should().Be(sale.Id);
        saleModifiedEvent.SaleNumber.Should().Be(sale.SaleNumber);
        saleModifiedEvent.CustomerId.Should().Be(sale.CustomerId);
        saleModifiedEvent.BranchId.Should().Be(sale.BranchId);
        saleModifiedEvent.Status.Should().Be(sale.Status);
        saleModifiedEvent.TotalAmount.Should().Be(sale.TotalAmount);
        saleModifiedEvent.EventTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EventMappingProfile_Should_Map_Sale_To_SaleCancelledEvent()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var sale = new Sale("SALE003", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        sale.Cancel();

        // Act
        var saleCancelledEvent = mapper.Map<SaleCancelledEvent>(sale);

        // Assert
        saleCancelledEvent.Should().NotBeNull();
        saleCancelledEvent.SaleId.Should().Be(sale.Id);
        saleCancelledEvent.SaleNumber.Should().Be(sale.SaleNumber);
        saleCancelledEvent.CustomerId.Should().Be(sale.CustomerId);
        saleCancelledEvent.BranchId.Should().Be(sale.BranchId);
        saleCancelledEvent.TotalAmount.Should().Be(sale.TotalAmount);
        saleCancelledEvent.EventTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EventMappingProfile_Should_Map_SaleItem_To_SaleItemCancelledEvent()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
        var mapper = config.CreateMapper();

        var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.50m, 0);

        // Act
        var saleItemCancelledEvent = mapper.Map<SaleItemCancelledEvent>(saleItem);

        // Assert
        saleItemCancelledEvent.Should().NotBeNull();
        saleItemCancelledEvent.SaleItemId.Should().Be(saleItem.Id);
        saleItemCancelledEvent.SaleId.Should().Be(saleItem.SaleId);
        saleItemCancelledEvent.ProductId.Should().Be(saleItem.ProductId);
        saleItemCancelledEvent.Quantity.Should().Be(saleItem.Quantity);
        saleItemCancelledEvent.UnitPrice.Should().Be(saleItem.UnitPrice);
        saleItemCancelledEvent.TotalAmount.Should().Be(saleItem.TotalAmount);
        saleItemCancelledEvent.EventTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EventTopics_Should_Have_Correct_Values()
    {
        // Assert
        EventTopics.SaleCreated.Should().Be("sale.created");
        EventTopics.SaleModified.Should().Be("sale.modified");
        EventTopics.SaleCancelled.Should().Be("sale.cancelled");
        EventTopics.SaleStatusChanged.Should().Be("sale.status.changed");
        EventTopics.SaleItemCancelled.Should().Be("sale.item.cancelled");
    }
} 