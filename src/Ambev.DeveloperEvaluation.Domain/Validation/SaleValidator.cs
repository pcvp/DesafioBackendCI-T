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
    /// - TotalAmount: Must be greater than or equal to 0
    /// - Items: Must have at least one item
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

        RuleFor(sale => sale.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative");

        RuleFor(sale => sale.Status)
            .IsInEnum()
            .WithMessage("Invalid sale status");
    }
} 