using Ambev.DeveloperEvaluation.Application.Base;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : BaseCommandHandler, IRequestHandler<DeleteSaleCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public DeleteSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the DeleteSaleCommand request
    /// </summary>
    /// <param name="command">The DeleteSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit indicating completion</returns>
    public async Task<Unit> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"DeleteSaleHandler: Deleting sale with ID '{command.Id}'");

        if (command.Id == Guid.Empty)
        {
            throw new ArgumentException("Sale ID cannot be empty", nameof(command.Id));
        }

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        await _saleRepository.DeleteAsync(command.Id, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale deletion transaction");

        Console.WriteLine($"DeleteSaleHandler: Sale with ID '{command.Id}' deleted successfully");

        return Unit.Value;
    }
} 