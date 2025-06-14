using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Validator for GetProductsQuery
/// </summary>
public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    /// <summary>
    /// Initializes validation rules for GetProductsQuery
    /// </summary>
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Size cannot exceed 100");

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithMessage("Search term cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Search));
    }
} 