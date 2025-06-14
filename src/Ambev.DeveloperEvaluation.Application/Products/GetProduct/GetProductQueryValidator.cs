using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Validator for GetProductQuery
/// </summary>
public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    /// <summary>
    /// Initializes validation rules for GetProductQuery
    /// </summary>
    public GetProductQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
} 