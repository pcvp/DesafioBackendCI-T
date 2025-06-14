using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductCommand
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateProductCommand
    /// </summary>
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .Length(2, 100)
            .WithMessage("Product name must be between 2 and 100 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Product price cannot exceed 999,999.99");
    }
} 