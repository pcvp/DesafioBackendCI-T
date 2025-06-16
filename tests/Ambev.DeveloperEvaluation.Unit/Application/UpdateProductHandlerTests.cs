using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="UpdateProductHandler"/> class.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateProductHandler(_productRepository, _mapper, _unitOfWork);
    }

    /// <summary>
    /// Tests that a valid product update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When updating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = ProductTestData.GenerateValidUpdateCommand();
        var existingProduct = new Ambev.DeveloperEvaluation.Domain.Entities.Product(command.Name, command.Price)
        {
            Id = command.Id
        };
        var updatedProduct = new Ambev.DeveloperEvaluation.Domain.Entities.Product(command.Name, command.Price)
        {
            Id = command.Id
        };
        var result = new UpdateProductResult
        {
            Id = command.Id,
            Name = command.Name,
            Price = command.Price,
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.UpdateAsync(Arg.Any<Ambev.DeveloperEvaluation.Domain.Entities.Product>(), Arg.Any<CancellationToken>())
            .Returns(updatedProduct);
        _mapper.Map<UpdateProductResult>(updatedProduct).Returns(result);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var updateResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(command.Id);
        updateResult.Name.Should().Be(command.Name);
        updateResult.Price.Should().Be(command.Price);
        updateResult.IsActive.Should().BeTrue();
        updateResult.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that an invalid product update request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When updating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new UpdateProductCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that validation is performed for empty name.
    /// </summary>
    [Fact(DisplayName = "Given command with empty name When handling Then throws validation exception")]
    public async Task Handle_EmptyName_ThrowsValidationException()
    {
        // Given
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Price = 10.50m
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateProductCommand.Name)));
    }

    /// <summary>
    /// Tests that validation is performed for invalid price.
    /// </summary>
    [Fact(DisplayName = "Given command with zero price When handling Then throws validation exception")]
    public async Task Handle_ZeroPrice_ThrowsValidationException()
    {
        // Given
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Valid Product Name",
            Price = 0
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateProductCommand.Price)));
    }

    /// <summary>
    /// Tests that validation is performed for empty GUID.
    /// </summary>
    [Fact(DisplayName = "Given command with empty GUID When handling Then throws validation exception")]
    public async Task Handle_EmptyGuid_ThrowsValidationException()
    {
        // Given
        var command = new UpdateProductCommand
        {
            Id = Guid.Empty,
            Name = "Valid Product Name",
            Price = 10.50m
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateProductCommand.Id)));
    }

    /// <summary>
    /// Tests that negative price validation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given command with negative price When handling Then throws validation exception")]
    public async Task Handle_NegativePrice_ThrowsValidationException()
    {
        // Given
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Valid Product Name",
            Price = -10.50m
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(UpdateProductCommand.Price)));
    }
} 