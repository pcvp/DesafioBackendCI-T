using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Validator for GetProductsRequest
/// </summary>
public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
    /// <summary>
    /// Initializes validation rules for GetProductsRequest
    /// </summary>
    public GetProductsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name filter cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));
    }
} 