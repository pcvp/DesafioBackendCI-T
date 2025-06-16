using Ambev.DeveloperEvaluation.Application.Base;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleStatus;

/// <summary>
/// Handler for processing UpdateSaleStatusCommand requests
/// </summary>
public class UpdateSaleStatusHandler : BaseCommandHandler, IRequestHandler<UpdateSaleStatusCommand>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Initializes a new instance of UpdateSaleStatusHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="messagePublisher">The message publisher</param>
    public UpdateSaleStatusHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IMapper mapper, IMessagePublisher messagePublisher) : base(unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    /// <summary>
    /// Handles the UpdateSaleStatusCommand request
    /// </summary>
    /// <param name="command">The UpdateSaleStatus command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task Handle(UpdateSaleStatusCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleStatusCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdWithItemsAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {command.Id} not found");

        

        switch (command.Status)
        {
            case SaleStatusEnum.Closed:
                if (!sale.Items.Any())
                    throw new InvalidOperationException($"Sale with ID {command.Id} has no items");

                if (sale.Status != SaleStatusEnum.Pending)
                    throw new InvalidOperationException($"Sale with ID {command.Id} has not pending status.");

                // Apply automatic discount rules when closing the sale
                ApplyAutomaticDiscountRules(sale);
                
                sale.Close();
                break;

            case SaleStatusEnum.Cancelled:
                sale.Cancel();
                break;

            case SaleStatusEnum.Paid:
                sale.Pay();
                break;

            case SaleStatusEnum.Pending:
                sale.Reactivate();
                break;

            default:
                throw new InvalidOperationException($"Invalid status: {command.Status}");
        }

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale status update transaction");

        // Publish SaleStatusChanged event after successful commit
        try
        {
            var saleStatusChangedEvent = _mapper.Map<SaleStatusChangedEvent>(sale);
            await _messagePublisher.PublishAsync(EventTopics.SaleStatusChanged, saleStatusChangedEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UpdateSaleStatusHandler: Failed to publish SaleStatusChanged event for sale {sale.Id}: {ex.Message}");
            // Log error but don't fail the operation since the sale was already committed
        }
    }

    /// <summary>
    /// Applies automatic discount rules based on item quantities
    /// </summary>
    /// <param name="sale">The sale to apply discounts to</param>
    private static void ApplyAutomaticDiscountRules(Sale sale)
    {
        // Group items by ProductId to calculate total quantity per product
        var productGroups = sale.Items
            .Where(i => !i.IsCancelled)
            .GroupBy(i => i.ProductId)
            .ToList();

        foreach (var group in productGroups)
        {
            var totalQuantity = group.Sum(i => i.Quantity);
            
            // Validate: Cannot sell more than 20 identical items
            if (totalQuantity > 20)
                throw new InvalidOperationException($"Cannot sell more than 20 identical items. Product {group.Key} has {totalQuantity} items.");

            // Calculate discount percentage based on quantity
            var discountPercentage = totalQuantity switch
            {
                >= 4 and <= 9 => 10m,
                >= 10 and <= 20 => 20m,
                _ => 0m
            };

            // Apply discount to all items of this product
            foreach (var item in group)
            {
                item.ApplyDiscount(discountPercentage);
            }
        }
    }
} 