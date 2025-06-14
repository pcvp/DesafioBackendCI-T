using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new DeleteSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid delete sale request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When deleting sale Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var command = SaleTestData.GenerateValidDeleteCommand();

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().Be(MediatR.Unit.Value);
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

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
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

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().Be(MediatR.Unit.Value);
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

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }
} 