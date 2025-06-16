using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for SaleItem entity that defines validation rules for sale item data.
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    /// <summary>
    /// Initializes a new instance of the SaleItemValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleId: Required, must not be empty
    /// - ProductId: Required, must not be empty
    /// - Quantity: Must be greater than 0 and not exceed 20 items
    /// - UnitPrice: Must be greater than 0 and not exceed $10,000
    /// - Discount: Must be between 0 and 100 (percentage)
    /// </remarks>
    public SaleItemValidator()
    {
        RuleFor(item => item.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity cannot exceed 20 items per sale item");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0")
            .LessThanOrEqualTo(10000)
            .WithMessage("Unit price cannot exceed $10,000");

        RuleFor(item => item.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");

        RuleFor(item => item.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative");
    }
} 