using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Validator for UpdateSaleItemCommand
/// </summary>
public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleItemCommand
    /// </summary>
    public UpdateSaleItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale item ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");
    }
} 