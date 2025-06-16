using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Domain.Uow;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteSaleHandler(_saleRepository, _unitOfWork);
    }

    /// <summary>
    /// Tests that a valid delete sale request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When deleting sale Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var command = SaleTestData.GenerateValidDeleteCommand();
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.Id;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().Be(MediatR.Unit.Value);
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that deleting non-existent sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When deleting Then throws exception")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Given
        var command = SaleTestData.GenerateValidDeleteCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that invalid delete request throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid delete data When deleting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new DeleteSaleCommand { Id = Guid.Empty };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Sale ID cannot be empty*");
    }

    /// <summary>
    /// Tests that deleting sale works correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When deleting Then deletes successfully")]
    public async Task Handle_ValidSale_DeletesSuccessfully()
    {
        // Given
        var command = SaleTestData.GenerateValidDeleteCommand();
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.Id;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().Be(MediatR.Unit.Value);
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that repository delete failure throws exception.
    /// </summary>
    [Fact(DisplayName = "Given repository delete failure When deleting Then throws exception")]
    public async Task Handle_RepositoryDeleteFailure_ThrowsException()
    {
        // Given
        var command = SaleTestData.GenerateValidDeleteCommand();
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.Id;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);
        _unitOfWork.Commit(Arg.Any<CancellationToken>()).Returns(false); // Simulate commit failure

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to commit sale deletion transaction");
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }
} 