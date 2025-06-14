using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public DeleteSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
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

        var deleted = await _saleRepository.DeleteAsync(command.Id, cancellationToken);
        if (!deleted)
        {
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
        }

        Console.WriteLine($"DeleteSaleHandler: Sale with ID '{command.Id}' deleted successfully");

        return Unit.Value;
    }
} 