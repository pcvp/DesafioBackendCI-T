using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Handler for processing CreateSaleItemCommand requests
/// </summary>
public class CreateSaleItemHandler : BaseCommandHandler, IRequestHandler<CreateSaleItemCommand, CreateSaleItemResult>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateSaleItemHandler
    /// </summary>
    /// <param name="saleItemRepository">The sale item repository</param>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    public CreateSaleItemHandler(ISaleItemRepository saleItemRepository, ISaleRepository saleRepository, IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _saleItemRepository = saleItemRepository;
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleItemCommand request
    /// </summary>
    /// <param name="command">The CreateSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item result</returns>
    public async Task<CreateSaleItemResult> Handle(CreateSaleItemCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"CreateSaleItemHandler: Creating sale item for sale '{command.SaleId}' with product '{command.ProductId}'");

        var validator = new CreateSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Verify that the sale exists and is in pending status
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {command.SaleId} not found");

        if (sale.Status != SaleStatusEnum.Pending)
            throw new InvalidOperationException($"Cannot add items to a sale with status {sale.Status}");

        // Verify that the product exists and is active
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {command.ProductId} not found");

        if (!product.IsActive)
            throw new InvalidOperationException($"Product with ID {command.ProductId} is not active");

        // Create sale item entity using the command values
        var saleItem = new SaleItem(
            command.SaleId,
            command.ProductId,
            command.Quantity,
            command.UnitPrice, // Use command's unit price
            command.Discount // Use command's discount
        );

        // Validate domain entity
        var domainValidation = saleItem.Validate();
        if (!domainValidation.IsValid)
            throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Save to repository
        var createdSaleItem = await _saleItemRepository.CreateAsync(saleItem, cancellationToken);
        var result = _mapper.Map<CreateSaleItemResult>(createdSaleItem);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit sale item creation transaction");

        Console.WriteLine($"CreateSaleItemHandler: Sale item created successfully with ID '{result.Id}' and total amount '{result.TotalAmount:C}'");

        return result;
    }
} 