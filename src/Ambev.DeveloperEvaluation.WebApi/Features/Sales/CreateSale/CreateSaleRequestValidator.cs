using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateSaleRequest
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required")
            .MaximumLength(50)
            .WithMessage("Sale number cannot exceed 50 characters");

        RuleFor(x => x.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future");

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        // Validate Items collection if provided
        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemRequestValidator())
            .When(x => x.Items != null && x.Items.Any());
    }
}

/// <summary>
/// Validator for CreateSaleItemRequest
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateSaleItemRequest
    /// </summary>
    public CreateSaleItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20) // Based on SaleItem entity validation
            .WithMessage("Quantity cannot exceed 20 units per item");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero")
            .LessThanOrEqualTo(10000) // Based on SaleItem entity validation
            .WithMessage("Unit price cannot exceed $10,000");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");
    }
} 