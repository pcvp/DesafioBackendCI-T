using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItems;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the GetSaleItemsHandler class.
/// Tests cover the handler's behavior with valid and invalid inputs,
/// including validation, business logic, and error scenarios.
/// </summary>
public class GetSaleItemsHandlerTests
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleItemsHandler _handler;

    /// <summary>
    /// Initializes a new instance of the test class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSaleItemsHandlerTests()
    {
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleItemsHandler(_saleItemRepository, _saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid get sale items request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should get sale items successfully with valid data")]
    public async Task Given_ValidGetSaleItemsQuery_When_Handle_Then_ShouldGetSaleItemsSuccessfully()
    {
        // Arrange
        var query = SaleItemTestData.GenerateValidGetSaleItemsQuery();
        var sale = CreateValidSale();
        var saleItems = new List<SaleItem>
        {
            new SaleItem(query.SaleId, Guid.NewGuid(), 2, 100m, 10m),
            new SaleItem(query.SaleId, Guid.NewGuid(), 1, 50m, 0m)
        };
        var expectedResult = SaleItemTestData.GenerateValidGetSaleItemsResult(2);

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>())
            .Returns((saleItems, 2));
        _mapper.Map<List<SaleItemResultDto>>(saleItems)
            .Returns(expectedResult.Items.ToList());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.CurrentPage.Should().Be(query.Page);
        result.TotalCount.Should().Be(2);
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<SaleItemResultDto>>(saleItems);
    }

    /// <summary>
    /// Tests that empty result is returned when no sale items exist for the sale.
    /// </summary>
    [Fact(DisplayName = "Handle should return empty result when no sale items exist")]
    public async Task Given_ValidQuery_When_NoSaleItemsExist_Then_ShouldReturnEmptyResult()
    {
        // Arrange
        var query = SaleItemTestData.GenerateValidGetSaleItemsQuery();
        var sale = CreateValidSale();
        var emptySaleItems = new List<SaleItem>();

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>())
            .Returns((emptySaleItems, 0));
        _mapper.Map<List<SaleItemResultDto>>(emptySaleItems)
            .Returns(new List<SaleItemResultDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.CurrentPage.Should().Be(query.Page);
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(1);
        result.HasNext.Should().BeFalse();
        result.HasPrevious.Should().BeFalse();
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<SaleItemResultDto>>(emptySaleItems);
    }

    /// <summary>
    /// Tests that an exception is thrown when the sale does not exist.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when sale not found")]
    public async Task Given_ValidQuery_When_SaleNotFound_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var query = SaleItemTestData.GenerateValidGetSaleItemsQuery();
        
        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(query, CancellationToken.None));
        
        exception.Message.Should().Be($"Sale with ID {query.SaleId} not found");
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.DidNotReceive().GetBySaleIdPagedAsync(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that pagination works correctly with multiple pages.
    /// </summary>
    [Fact(DisplayName = "Handle should handle pagination correctly")]
    public async Task Given_QueryWithPagination_When_Handle_Then_ShouldReturnCorrectPaginationInfo()
    {
        // Arrange
        var query = SaleItemTestData.GenerateGetSaleItemsQuery(Guid.NewGuid(), 2, 5); // Page 2, Size 5
        var sale = CreateValidSale();
        var saleItems = new List<SaleItem>
        {
            new SaleItem(query.SaleId, Guid.NewGuid(), 1, 60m, 0m),
            new SaleItem(query.SaleId, Guid.NewGuid(), 2, 70m, 5m)
        };

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>())
            .Returns((saleItems, 12)); // Total 12 items
        _mapper.Map<List<SaleItemResultDto>>(saleItems)
            .Returns(new List<SaleItemResultDto>
            {
                new SaleItemResultDto { Id = Guid.NewGuid(), ProductId = Guid.NewGuid() },
                new SaleItemResultDto { Id = Guid.NewGuid(), ProductId = Guid.NewGuid() }
            });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.CurrentPage.Should().Be(2);
        result.TotalCount.Should().Be(12);
        result.TotalPages.Should().Be(3); // 12 items / 5 per page = 3 pages
        result.HasNext.Should().BeTrue();
        result.HasPrevious.Should().BeTrue();
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct sale items.
    /// </summary>
    [Fact(DisplayName = "Handle should call mapper with found sale items")]
    public async Task Given_ValidQuery_When_Handle_Then_ShouldCallMapperWithFoundSaleItems()
    {
        // Arrange
        var query = SaleItemTestData.GenerateValidGetSaleItemsQuery();
        var sale = CreateValidSale();
        var saleItems = new List<SaleItem>
        {
            new SaleItem(query.SaleId, Guid.NewGuid(), 2, 100m, 10m)
        };
        var mappedItems = new List<SaleItemResultDto>
        {
            new SaleItemResultDto { Id = Guid.NewGuid(), ProductId = Guid.NewGuid() }
        };

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>())
            .Returns((saleItems, 1));
        _mapper.Map<List<SaleItemResultDto>>(saleItems)
            .Returns(mappedItems);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<List<SaleItemResultDto>>(saleItems);
        result.Items.Should().BeEquivalentTo(mappedItems);
    }

    /// <summary>
    /// Tests that cancelled sale items are included in the results.
    /// </summary>
    [Fact(DisplayName = "Handle should include cancelled sale items in results")]
    public async Task Given_ValidQuery_When_SaleItemsIncludeCancelled_Then_ShouldIncludeAllItems()
    {
        // Arrange
        var query = SaleItemTestData.GenerateValidGetSaleItemsQuery();
        var sale = CreateValidSale();
        var activeSaleItem = new SaleItem(query.SaleId, Guid.NewGuid(), 2, 100m, 10m);
        var cancelledSaleItem = new SaleItem(query.SaleId, Guid.NewGuid(), 1, 50m, 0m);
        cancelledSaleItem.Cancel();
        
        var saleItems = new List<SaleItem> { activeSaleItem, cancelledSaleItem };
        var mappedItems = new List<SaleItemResultDto>
        {
            new SaleItemResultDto { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), IsCancelled = false },
            new SaleItemResultDto { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), IsCancelled = true }
        };

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>())
            .Returns((saleItems, 2));
        _mapper.Map<List<SaleItemResultDto>>(saleItems)
            .Returns(mappedItems);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(item => item.IsCancelled == false);
        result.Items.Should().Contain(item => item.IsCancelled == true);
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).GetBySaleIdPagedAsync(query.SaleId, query.Page, query.Size, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that default pagination values work correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should use default pagination values when not specified")]
    public async Task Given_QueryWithDefaultPagination_When_Handle_Then_ShouldUseDefaultValues()
    {
        // Arrange
        var query = new GetSaleItemsQuery
        {
            SaleId = Guid.NewGuid(),
            Page = 1,
            Size = 10
        };
        var sale = CreateValidSale();
        var saleItems = new List<SaleItem>
        {
            new SaleItem(query.SaleId, Guid.NewGuid(), 1, 100m, 0m)
        };

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, 1, 10, Arg.Any<CancellationToken>())
            .Returns((saleItems, 1));
        _mapper.Map<List<SaleItemResultDto>>(saleItems)
            .Returns(new List<SaleItemResultDto> { new SaleItemResultDto() });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(1);
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).GetBySaleIdPagedAsync(query.SaleId, 1, 10, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that large page sizes are handled correctly.
    /// </summary>
    [Theory(DisplayName = "Handle should work with different page sizes")]
    [InlineData(1, 5)]
    [InlineData(2, 10)]
    [InlineData(3, 20)]
    [InlineData(1, 50)]
    public async Task Given_QueryWithDifferentPageSizes_When_Handle_Then_ShouldHandleCorrectly(
        int page, int size)
    {
        // Arrange
        var query = SaleItemTestData.GenerateGetSaleItemsQuery(Guid.NewGuid(), page, size);
        var sale = CreateValidSale();
        var saleItems = new List<SaleItem>
        {
            new SaleItem(query.SaleId, Guid.NewGuid(), 1, 100m, 0m)
        };

        _saleRepository.GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleItemRepository.GetBySaleIdPagedAsync(query.SaleId, page, size, Arg.Any<CancellationToken>())
            .Returns((saleItems, 1));
        _mapper.Map<List<SaleItemResultDto>>(saleItems)
            .Returns(new List<SaleItemResultDto> { new SaleItemResultDto() });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        await _saleRepository.Received(1).GetByIdAsync(query.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).GetBySaleIdPagedAsync(query.SaleId, page, size, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Helper method to create a valid Sale entity.
    /// </summary>
    private static Sale CreateValidSale()
    {
        var sale = new Sale();
        sale.UpdateSaleInfo("SALE001", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        return sale;
    }
} 