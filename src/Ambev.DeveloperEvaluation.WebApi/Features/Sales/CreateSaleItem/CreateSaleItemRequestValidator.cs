using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem;

/// <summary>
/// Validator for AddSaleItemRequest
/// </summary>
public class AddSaleItemRequestValidator : AbstractValidator<AddSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for AddSaleItemRequest
    /// </summary>
    public AddSaleItemRequestValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");
    }
} 