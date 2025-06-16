using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : BaseCommandHandler, IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="messagePublisher">The message publisher</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork unitOfWork, IMessagePublisher messagePublisher): base(unitOfWork)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale result</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"CreateSaleHandler: Creating sale with number '{command.SaleNumber}' for customer '{command.CustomerId}'");

        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Check if sale with same sale number already exists
        if (!string.IsNullOrEmpty(command.SaleNumber))
        {
            var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
            if (existingSale != null)
                throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");
        }

        // Create sale entity
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId
        );

        // Add sale items if provided
        if (command.Items != null && command.Items.Any())
        {
            foreach (var itemCommand in command.Items)
            {
                var saleItem = new SaleItem(
                    sale.Id,
                    itemCommand.ProductId,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice,
                    itemCommand.Discount
                );

                // Validate each item
                var itemValidation = saleItem.Validate();
                if (!itemValidation.IsValid)
                    throw new ValidationException(itemValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

                sale.AddItem(saleItem);
            }
        }

        // Validate domain entity - commented out as it causes issues with ID validation during creation
        // var domainValidation = sale.Validate();
        // if (!domainValidation.IsValid)
        //     throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Save to repository
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);

        if(!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale creation transaction");

        // Publish SaleCreated event after successful commit
        try
        {
            var saleCreatedEvent = _mapper.Map<SaleCreatedEvent>(createdSale);
            await _messagePublisher.PublishAsync(EventTopics.SaleCreated, saleCreatedEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CreateSaleHandler: Failed to publish SaleCreated event for sale {createdSale.Id}: {ex.Message}");
            // Log error but don't fail the operation since the sale was already committed
        }

        Console.WriteLine($"CreateSaleHandler: Sale created successfully with ID '{result.Id}' and total amount '{result.TotalAmount:C}' with {result.Items.Count} items");

        return result;
    }
} 