using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSaleItem;

/// <summary>
/// Validator for DeleteSaleItemRequest
/// </summary>
public class DeleteSaleItemRequestValidator : AbstractValidator<DeleteSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for DeleteSaleItemRequest
    /// </summary>
    public DeleteSaleItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale item ID is required");

        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 