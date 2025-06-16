using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

/// <summary>
/// Handler for processing DeleteSaleItemCommand requests
/// </summary>
public class DeleteSaleItemHandler : BaseCommandHandler, IRequestHandler<DeleteSaleItemCommand>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Initializes a new instance of DeleteSaleItemHandler
    /// </summary>
    /// <param name="saleItemRepository">The sale item repository</param>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="messagePublisher">The message publisher</param>
    public DeleteSaleItemHandler(ISaleItemRepository saleItemRepository, ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork, IMessagePublisher messagePublisher) : base(unitOfWork)
    {
        _saleItemRepository = saleItemRepository;
        _saleRepository = saleRepository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    /// <summary>
    /// Handles the DeleteSaleItemCommand request
    /// </summary>
    /// <param name="command">The DeleteSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public async Task Handle(DeleteSaleItemCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"DeleteSaleItemHandler: Deleting sale item '{command.Id}' from sale '{command.SaleId}'");

        // Verify that the sale exists
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {command.SaleId} not found");

        // Verify that the sale item exists
        var saleItem = await _saleItemRepository.GetByIdAsync(command.Id, cancellationToken);
        if (saleItem == null)
            throw new KeyNotFoundException($"Sale item with ID {command.Id} not found");

        // Verify that the sale item belongs to the specified sale
        if (saleItem.SaleId != command.SaleId)
            throw new InvalidOperationException($"Sale item with ID {command.Id} does not belong to sale {command.SaleId}");

        // Check if sale is in a state that allows item deletion
        if (sale.Status != Domain.Entities.SaleStatusEnum.Pending)
            throw new InvalidOperationException($"Cannot delete items from a sale with status {sale.Status}");

        await _saleItemRepository.DeleteAsync(command.Id, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale item deletion transaction");

        // Publish SaleItemCancelled event after successful commit
        try
        {
            var saleItemCancelledEvent = _mapper.Map<SaleItemCancelledEvent>(saleItem);
            await _messagePublisher.PublishAsync(EventTopics.SaleItemCancelled, saleItemCancelledEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DeleteSaleItemHandler: Failed to publish SaleItemCancelled event for sale item {saleItem.Id}: {ex.Message}");
            // Log error but don't fail the operation since the sale item was already committed
        }

        Console.WriteLine($"DeleteSaleItemHandler: Sale item '{command.Id}' deleted successfully from sale '{command.SaleId}'");
    }
} 