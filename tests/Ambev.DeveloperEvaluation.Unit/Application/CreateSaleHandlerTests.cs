using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _unitOfWork, _messagePublisher);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>()
        };
        
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId)
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
            TotalAmount = sale.TotalAmount,
            Status = sale.Status,
            Items = new List<CreateSaleItemResult>(),
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        createSaleResult.SaleNumber.Should().Be(command.SaleNumber);
        createSaleResult.TotalAmount.Should().Be(sale.TotalAmount);
        createSaleResult.Status.Should().Be(sale.Status);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = string.Empty, // Invalid: empty sale number
            SaleDate = DateTime.UtcNow.AddDays(1), // Invalid: future date
            CustomerId = Guid.Empty, // Invalid: empty customer ID
            BranchId = Guid.Empty, // Invalid: empty branch ID
            Items = new List<CreateSaleItemCommand>() // Empty items list
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the sale creation throws exception when sale number already exists.
    /// </summary>
    [Fact(DisplayName = "Given duplicate sale number When creating sale Then throws exception")]
    public async Task Handle_DuplicateSaleNumber_ThrowsException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "DUPLICATE001",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>() // Empty items list
        };
        
        var existingSale = new Sale(
            command.SaleNumber,
            command.SaleDate.AddDays(-1),
            Guid.NewGuid(),
            Guid.NewGuid())
        {
            Id = Guid.NewGuid()
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with number {command.SaleNumber} already exists");
    }

    /// <summary>
    /// Tests that the sale creation handles null items list correctly.
    /// </summary>
    [Fact(DisplayName = "Given command with null items When handling Then creates sale successfully")]
    public async Task Handle_NullItems_CreatesSaleSuccessfully()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE005",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = null // Null items
        };
        
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId)
        {
            Id = Guid.NewGuid()
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.SaleNumber == command.SaleNumber &&
                s.Items.Count == 0),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the sale is created with correct properties.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates sale with correct properties")]
    public async Task Handle_ValidRequest_CreatesSaleWithCorrectProperties()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE002",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>()
        };
        
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId)
        {
            Id = Guid.NewGuid()
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(new CreateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.SaleNumber == command.SaleNumber &&
                s.SaleDate == command.SaleDate &&
                s.CustomerId == command.CustomerId &&
                s.BranchId == command.BranchId &&
                s.Status != Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Cancelled),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the created sale.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps sale to result")]
    public async Task Handle_ValidRequest_MapsSaleToResult()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE003",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>()
        };
        
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId)
        {
            Id = Guid.NewGuid()
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

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
            Items = new List<CreateSaleItemCommand>()
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateSaleCommand.SaleNumber)));
    }

    /// <summary>
    /// Tests that validation is performed for invalid item data.
    /// </summary>
    [Fact(DisplayName = "Given command with invalid item When handling Then throws validation exception")]
    public async Task Handle_InvalidItem_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = Guid.Empty, // Invalid: empty product ID
                    Quantity = 0, // Invalid: zero quantity
                    UnitPrice = -10.50m, // Invalid: negative price
                    Discount = 150 // Invalid: discount over 100%
                }
            }
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that sale can be created without items.
    /// </summary>
    [Fact(DisplayName = "Given command without items When handling Then creates sale successfully")]
    public async Task Handle_CommandWithoutItems_CreatesSaleSuccessfully()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE004",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>()
        };
        
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId)
        {
            Id = Guid.NewGuid()
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.SaleNumber == command.SaleNumber &&
                s.Items.Count == 0),
            Arg.Any<CancellationToken>());
    }
} 