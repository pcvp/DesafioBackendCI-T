using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItem;

/// <summary>
/// Validator for GetSaleItemRequest
/// </summary>
public class GetSaleItemRequestValidator : AbstractValidator<GetSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for GetSaleItemRequest
    /// </summary>
    public GetSaleItemRequestValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale item ID is required");
    }
} 