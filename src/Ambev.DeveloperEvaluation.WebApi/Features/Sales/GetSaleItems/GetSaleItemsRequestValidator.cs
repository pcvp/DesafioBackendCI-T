using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItems;

/// <summary>
/// Validator for GetSaleItemsRequest
/// </summary>
public class GetSaleItemsRequestValidator : AbstractValidator<GetSaleItemsRequest>
{
    /// <summary>
    /// Initializes validation rules for GetSaleItemsRequest
    /// </summary>
    public GetSaleItemsRequestValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Size cannot exceed 100");
    }
} 