using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : BaseCommandHandler, IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="messagePublisher">The message publisher</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork, IMessagePublisher messagePublisher) : base(unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale result</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"UpdateSaleHandler: Updating sale with ID '{command.Id}' and number '{command.SaleNumber}'");

        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Get existing sale
        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        // Check if sale number is being changed and if it already exists
        if (existingSale.SaleNumber != command.SaleNumber)
        {
            var saleWithSameNumber = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
            if (saleWithSameNumber != null && saleWithSameNumber.Id != command.Id)
                throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");
        }

        // Track if sale is being cancelled
        var wasActive = existingSale.Status != SaleStatusEnum.Cancelled;
        var willBeCancelled = command.Status == SaleStatusEnum.Cancelled;

        // Update status first if changed (especially important for reactivating cancelled sales)
        if (existingSale.Status != command.Status)
        {
            switch (command.Status)
            {
                case SaleStatusEnum.Cancelled:
                    existingSale.Cancel();
                    break;
                case SaleStatusEnum.Pending:
                    if (existingSale.Status == SaleStatusEnum.Cancelled)
                        existingSale.Reactivate();
                    break;
                case SaleStatusEnum.Paid:
                    existingSale.Pay();
                    break;
                case SaleStatusEnum.Closed:
                    existingSale.Close();
                    break;
            }
        }

        // Update sale information only if not changing to cancelled status
        if (command.Status != SaleStatusEnum.Cancelled)
        {
            existingSale.UpdateSaleInfo(
                command.SaleNumber,
                command.SaleDate,
                command.CustomerId,
                command.BranchId
            );

            // Update or create sale item
            var existingItem = existingSale.Items.FirstOrDefault();
            if (existingItem != null)
            {
                // Update existing item
                existingItem.UpdateItemInfo(
                    existingSale.Id,
                    command.ProductId,
                    command.Quantity,
                    command.UnitPrice,
                    command.Discount
                );
            }
            else
            {
                // Create new item if none exists
                var newItem = new SaleItem(
                    existingSale.Id,
                    command.ProductId,
                    command.Quantity,
                    command.UnitPrice,
                    command.Discount
                );
                existingSale.AddItem(newItem);
            }
        }

        // Validate domain entity
        var domainValidation = existingSale.Validate();
        if (!domainValidation.IsValid)
            throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Update in repository
        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(updatedSale);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale update transaction");

        // Publish appropriate events after successful commit
        try
        {
            if (wasActive && willBeCancelled)
            {
                // Sale was cancelled
                var saleCancelledEvent = _mapper.Map<SaleCancelledEvent>(updatedSale);
                await _messagePublisher.PublishAsync(EventTopics.SaleCancelled, saleCancelledEvent, cancellationToken);
            }
            else
            {
                // Sale was modified
                var saleModifiedEvent = _mapper.Map<SaleModifiedEvent>(updatedSale);
                await _messagePublisher.PublishAsync(EventTopics.SaleModified, saleModifiedEvent, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UpdateSaleHandler: Failed to publish event for sale {updatedSale.Id}: {ex.Message}");
            // Log error but don't fail the operation since the sale was already committed
        }

        Console.WriteLine($"UpdateSaleHandler: Sale updated successfully with ID '{result.Id}' and total amount '{result.TotalAmount:C}'");

        return result;
    }
} 