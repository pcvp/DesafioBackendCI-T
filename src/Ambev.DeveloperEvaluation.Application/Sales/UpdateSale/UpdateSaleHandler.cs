using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
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

        // Update sale information
        existingSale.UpdateSaleInfo(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.BranchId,
            command.ProductId,
            command.Quantity,
            command.UnitPrice,
            command.Discount
        );

        // Update cancellation status if changed
        if (existingSale.IsCancelled != command.IsCancelled)
        {
            if (command.IsCancelled)
                existingSale.Cancel();
            else
                existingSale.Reactivate();
        }

        // Validate domain entity
        var domainValidation = existingSale.Validate();
        if (!domainValidation.IsValid)
            throw new ValidationException(domainValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Error, e.Detail)));

        // Save to repository
        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(updatedSale);

        Console.WriteLine($"UpdateSaleHandler: Sale updated successfully with total amount '{result.TotalAmount:C}'");

        return result;
    }
} 