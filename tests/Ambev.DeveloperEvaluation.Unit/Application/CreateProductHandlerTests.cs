using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateProductHandler(_productRepository, _mapper, _unitOfWork);
    }

    /// <summary>
    /// Tests that a valid product creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = ProductTestData.GenerateValidCreateCommand();
        var product = new Product(command.Name, command.Price)
        {
            Id = Guid.NewGuid()
        };

        var result = new CreateProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<CreateProductResult>(product).Returns(result);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var createProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createProductResult.Should().NotBeNull();
        createProductResult.Id.Should().Be(product.Id);
        createProductResult.Name.Should().Be(command.Name);
        createProductResult.Price.Should().Be(command.Price);
        createProductResult.IsActive.Should().BeTrue();
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid product creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateProductCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the product is created with correct properties.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates product with correct properties")]
    public async Task Handle_ValidRequest_CreatesProductWithCorrectProperties()
    {
        // Given
        var command = ProductTestData.GenerateValidCreateCommand();
        var product = new Product(command.Name, command.Price)
        {
            Id = Guid.NewGuid()
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<CreateProductResult>(Arg.Any<Product>()).Returns(new CreateProductResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _productRepository.Received(1).CreateAsync(
            Arg.Is<Product>(p => 
                p.Name == command.Name &&
                p.Price == command.Price &&
                p.IsActive == true),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the created product.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps product to result")]
    public async Task Handle_ValidRequest_MapsProductToResult()
    {
        // Given
        var command = ProductTestData.GenerateValidCreateCommand();
        var product = new Product(command.Name, command.Price)
        {
            Id = Guid.NewGuid()
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<CreateProductResult>(product).Returns(new CreateProductResult());
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<CreateProductResult>(Arg.Is<Product>(p =>
            p.Name == command.Name &&
            p.Price == command.Price &&
            p.IsActive == true));
    }

    /// <summary>
    /// Tests that validation is performed before creating the product.
    /// </summary>
    [Fact(DisplayName = "Given command with empty name When handling Then throws validation exception")]
    public async Task Handle_EmptyName_ThrowsValidationException()
    {
        // Given
        var command = new CreateProductCommand
        {
            Name = string.Empty,
            Price = 10.50m
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateProductCommand.Name)));
    }

    /// <summary>
    /// Tests that validation is performed for invalid price.
    /// </summary>
    [Fact(DisplayName = "Given command with zero price When handling Then throws validation exception")]
    public async Task Handle_ZeroPrice_ThrowsValidationException()
    {
        // Given
        var command = new CreateProductCommand
        {
            Name = "Valid Product Name",
            Price = 0
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(CreateProductCommand.Price)));
    }
} 