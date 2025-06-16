using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

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
            .WithMessage("Quantity cannot exceed 20 items per sale item");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0")
            .LessThanOrEqualTo(10000)
            .WithMessage("Unit price cannot exceed $10,000");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");
    }
} 