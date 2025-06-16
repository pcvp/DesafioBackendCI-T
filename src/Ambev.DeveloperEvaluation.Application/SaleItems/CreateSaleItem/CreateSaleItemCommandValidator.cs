using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Validator for CreateSaleItemCommand
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes validation rules for CreateSaleItemCommand
    /// </summary>
    public CreateSaleItemCommandValidator()
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
            .WithMessage("Quantity cannot exceed 20 items per sale item");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");
    }
} 