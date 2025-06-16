using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteProductHandler"/> class.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteProductHandler(_productRepository, _unitOfWork);
    }

    /// <summary>
    /// Tests that a valid product deletion request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product ID When deleting product Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var command = ProductTestData.GenerateValidDeleteCommand();
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 10.50m) { Id = command.Id };
        
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _productRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().NotThrowAsync();
    }

    /// <summary>
    /// Tests that an invalid product deletion request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product ID When deleting product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new DeleteProductCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that validation is performed for empty GUID.
    /// </summary>
    [Fact(DisplayName = "Given command with empty GUID When handling Then throws validation exception")]
    public async Task Handle_EmptyGuid_ThrowsValidationException()
    {
        // Given
        var command = new DeleteProductCommand
        {
            Id = Guid.Empty
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.PropertyName == nameof(DeleteProductCommand.Id)));
    }

    /// <summary>
    /// Tests that a valid GUID passes validation.
    /// </summary>
    [Fact(DisplayName = "Given command with valid GUID When handling Then completes successfully")]
    public async Task Handle_ValidGuid_CompletesSuccessfully()
    {
        // Given
        var command = new DeleteProductCommand
        {
            Id = Guid.NewGuid()
        };
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 10.50m) { Id = command.Id };
        
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _productRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().NotThrowAsync();
    }

    /// <summary>
    /// Tests that the handler processes the command without throwing exceptions.
    /// </summary>
    [Fact(DisplayName = "Given valid delete command When handling Then processes without error")]
    public async Task Handle_ValidCommand_ProcessesWithoutError()
    {
        // Given
        var command = ProductTestData.GenerateValidDeleteCommand();
        var product = new Ambev.DeveloperEvaluation.Domain.Entities.Product("Test Product", 10.50m) { Id = command.Id };
        
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _productRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When & Then
        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));
        exception.Should().BeNull();
    }
} 