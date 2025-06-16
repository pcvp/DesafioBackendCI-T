using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

/// <summary>
/// Handler for processing UpdateSaleItemCommand requests
/// </summary>
public class UpdateSaleItemHandler : BaseCommandHandler, IRequestHandler<UpdateSaleItemCommand, UpdateSaleItemResult>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateSaleItemHandler
    /// </summary>
    /// <param name="saleItemRepository">The sale item repository</param>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    public UpdateSaleItemHandler(ISaleItemRepository saleItemRepository, ISaleRepository saleRepository, IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _saleItemRepository = saleItemRepository;
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateSaleItemCommand request
    /// </summary>
    /// <param name="command">The UpdateSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale item result</returns>
    public async Task<UpdateSaleItemResult> Handle(UpdateSaleItemCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"UpdateSaleItemHandler: Updating sale item '{command.Id}' with product '{command.ProductId}'");

        var validator = new UpdateSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Get existing sale item
        var existingSaleItem = await _saleItemRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSaleItem == null)
            throw new KeyNotFoundException($"Sale item with ID {command.Id} not found");

        // Verify that the sale exists and is in pending status
        var sale = await _saleRepository.GetByIdAsync(existingSaleItem.SaleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {existingSaleItem.SaleId} not found");

        if (sale.Status != Domain.Entities.SaleStatusEnum.Pending)
            throw new InvalidOperationException($"Cannot update items in a sale with status {sale.Status}");

        // Verify that the product exists and is active
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {command.ProductId} not found");

        if (!product.IsActive)
            throw new InvalidOperationException($"Product with ID {command.ProductId} is not active");

        // Update sale item information using the product's unit price
        existingSaleItem.UpdateItemInfo(
            existingSaleItem.SaleId,
            command.ProductId,
            command.Quantity,
            product.Price, // Use product's price instead of user input
            existingSaleItem.Discount // Keep existing discount
        );

        // Validate domain entity
        var domainValidation = existingSaleItem.Validate();
        if (!domainValidation.IsValid)
            throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Update in repository
        var updatedSaleItem = await _saleItemRepository.UpdateAsync(existingSaleItem, cancellationToken);
        var result = _mapper.Map<UpdateSaleItemResult>(updatedSaleItem);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale item update transaction");

        Console.WriteLine($"UpdateSaleItemHandler: Sale item updated successfully with ID '{result.Id}' and total amount '{result.TotalAmount:C}'");

        return result;
    }
} 