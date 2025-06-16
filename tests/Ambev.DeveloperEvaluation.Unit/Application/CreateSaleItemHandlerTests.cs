using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the CreateSaleItemHandler class.
/// Tests cover the handler's behavior with valid and invalid inputs,
/// including validation, business logic, and error scenarios.
/// </summary>
public class CreateSaleItemHandlerTests
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateSaleItemHandler _handler;

    /// <summary>
    /// Initializes a new instance of the test class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleItemHandlerTests()
    {
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateSaleItemHandler(_saleItemRepository, _saleRepository, _productRepository, _mapper, _unitOfWork);
    }

    /// <summary>
    /// Tests that a valid sale item creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should create sale item successfully with valid data")]
    public async Task Given_ValidCreateSaleItemCommand_When_Handle_Then_ShouldCreateSaleItemSuccessfully()
    {
        // Arrange
        var command = SaleItemTestData.GenerateValidCreateCommand();
        var sale = CreateValidSale();
        var product = CreateValidProduct();
        var createdSaleItem = new SaleItem(command.SaleId, command.ProductId, command.Quantity, product.Price, command.Discount);
        var expectedResult = SaleItemTestData.GenerateValidCreateSaleItemResult();

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleItemRepository.CreateAsync(Arg.Any<SaleItem>(), Arg.Any<CancellationToken>())
            .Returns(createdSaleItem);
        _mapper.Map<CreateSaleItemResult>(Arg.Any<SaleItem>())
            .Returns(expectedResult);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedResult);
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>());
        await _saleItemRepository.Received(1).CreateAsync(Arg.Any<SaleItem>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateSaleItemResult>(Arg.Any<SaleItem>());
    }

    /// <summary>
    /// Tests that validation errors are thrown when command validation fails.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Given_InvalidCreateSaleItemCommand_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = SaleItemTestData.GenerateInvalidCreateCommand();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        exception.Should().NotBeNull();
        exception.Errors.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that an exception is thrown when the sale does not exist.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when sale not found")]
    public async Task Given_ValidCommand_When_SaleNotFound_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = CreateValidSale();
        var command = SaleItemTestData.GenerateValidCreateCommand();
        command.SaleId = sale.Id;
        
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Be($"Sale with ID {command.SaleId} not found");
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
        await _saleItemRepository.DidNotReceive().CreateAsync(Arg.Any<SaleItem>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an exception is thrown when trying to add an item to a cancelled sale.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when sale is cancelled")]
    public async Task Given_ValidCommand_When_SaleIsCancelled_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = CreateValidSale();
        sale.Cancel(); // Cancel the sale
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 100m) { Id = Guid.NewGuid() };
        var command = new CreateSaleItemCommand
        {
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = 1,
            UnitPrice = 10m,
            Discount = 0m
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Be("Cannot add items to a sale with status Cancelled");
    }

    /// <summary>
    /// Tests that the handler creates a sale item with correct properties.
    /// </summary>
    [Fact(DisplayName = "Handle should create sale item with correct properties")]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldCreateSaleItemWithCorrectProperties()
    {
        // Arrange
        var sale = CreateValidSale();
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 100m) { Id = Guid.NewGuid() };
        var command = new CreateSaleItemCommand
        {
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = 2,
            UnitPrice = 100m,
            Discount = 10m
        };

        var expectedSaleItem = new SaleItem(command.SaleId, command.ProductId, command.Quantity, command.UnitPrice, command.Discount);
        var expectedResult = new CreateSaleItemResult
        {
            Id = expectedSaleItem.Id,
            SaleId = command.SaleId,
            ProductId = command.ProductId,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            Discount = command.Discount,
            TotalAmount = expectedSaleItem.TotalAmount
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleItemRepository.CreateAsync(Arg.Any<SaleItem>(), Arg.Any<CancellationToken>())
            .Returns(expectedSaleItem);
        _mapper.Map<CreateSaleItemResult>(expectedSaleItem)
            .Returns(expectedResult);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SaleId.Should().Be(command.SaleId);
        result.ProductId.Should().Be(command.ProductId);
        result.Quantity.Should().Be(command.Quantity);
        result.UnitPrice.Should().Be(command.UnitPrice);
        result.Discount.Should().Be(command.Discount);
    }

    /// <summary>
    /// Tests that the mapper is called with the created sale item.
    /// </summary>
    [Fact(DisplayName = "Handle should call mapper with created sale item")]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldCallMapperWithCreatedSaleItem()
    {
        // Arrange
        var sale = CreateValidSale();
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 100m) { Id = Guid.NewGuid() };
        var command = new CreateSaleItemCommand
        {
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = 2,
            UnitPrice = 100m,
            Discount = 10m
        };

        var expectedSaleItem = new SaleItem(command.SaleId, command.ProductId, command.Quantity, command.UnitPrice, command.Discount);
        var expectedResult = new CreateSaleItemResult();

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleItemRepository.CreateAsync(Arg.Any<SaleItem>(), Arg.Any<CancellationToken>())
            .Returns(expectedSaleItem);
        _mapper.Map<CreateSaleItemResult>(Arg.Any<SaleItem>())
            .Returns(expectedResult);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<CreateSaleItemResult>(Arg.Is<SaleItem>(x => 
            x.SaleId == command.SaleId &&
            x.ProductId == command.ProductId &&
            x.Quantity == command.Quantity &&
            x.UnitPrice == command.UnitPrice &&
            x.Discount == command.Discount));
    }

    /// <summary>
    /// Tests that the total amount is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should calculate total amount correctly")]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldCalculateTotalAmountCorrectly()
    {
        // Arrange
        var sale = CreateValidSale();
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 100m) { Id = Guid.NewGuid() };
        var command = new CreateSaleItemCommand
        {
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = 2,
            UnitPrice = 100m,
            Discount = 10m // 10% discount
        };
        var expectedTotalAmount = 2 * 100m * (1 - 10m / 100); // 2 * 100 * 0.9 = 180
        
        SaleItem? capturedSaleItem = null;
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleItemRepository.CreateAsync(Arg.Do<SaleItem>(x => capturedSaleItem = x), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<SaleItem>());
        _mapper.Map<CreateSaleItemResult>(Arg.Any<SaleItem>())
            .Returns(new CreateSaleItemResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedSaleItem.Should().NotBeNull();
        capturedSaleItem!.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that validation is performed for required fields.
    /// </summary>
    [Fact(DisplayName = "Handle should validate required fields")]
    public async Task Given_CommandWithEmptyProductId_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.Empty, // Empty ProductId
            Quantity = 1,
            UnitPrice = 10m,
            Discount = 0m
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        exception.Should().NotBeNull();
        exception.Errors.Should().NotBeEmpty();
    }

    /// <summary>
    /// Helper method to create a valid Sale entity.
    /// </summary>
    private static Sale CreateValidSale()
    {
        var sale = new Sale("SALE001", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid());
        sale.Id = Guid.NewGuid(); // Ensure the sale has a valid ID
        return sale;
    }

    /// <summary>
    /// Helper method to create a valid Product entity.
    /// </summary>
    private static Product CreateValidProduct()
    {
        return new Product("Test Product", 100.00m);
    }
} 