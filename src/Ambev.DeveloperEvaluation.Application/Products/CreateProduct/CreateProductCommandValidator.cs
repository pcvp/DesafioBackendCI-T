using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes validation rules for CreateProductCommand
    /// </summary>
    public CreateProductCommandValidator()
    {
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