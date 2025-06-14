using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Sale entity that defines validation rules for sale data.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    /// <summary>
    /// Initializes a new instance of the SaleValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, length between 1 and 50 characters
    /// - SaleDate: Required, cannot be in the future
    /// - CustomerId: Required, must not be empty GUID
    /// - BranchId: Required, must not be empty GUID
    /// - ProductId: Required, must not be empty GUID
    /// - Quantity: Must be greater than 0 and less than or equal to 1000
    /// - UnitPrice: Must be greater than 0 and less than or equal to 999,999.99
    /// - Discount: Must be between 0 and 100 (inclusive)
    /// </remarks>
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required")
            .MinimumLength(1)
            .WithMessage("Sale number must be at least 1 character long")
            .MaximumLength(50)
            .WithMessage("Sale number cannot be longer than 50 characters");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(sale => sale.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(sale => sale.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(1000)
            .WithMessage("Quantity cannot exceed 1000 units");

        RuleFor(sale => sale.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Unit price cannot exceed 999,999.99");

        RuleFor(sale => sale.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");

        RuleFor(sale => sale.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative");

        RuleFor(sale => sale.TotalSaleAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total sale amount cannot be negative");
    }
} 