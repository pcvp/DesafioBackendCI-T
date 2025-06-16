using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleStatus;

/// <summary>
/// Validator for UpdateSaleStatusRequest
/// </summary>
public class UpdateSaleStatusRequestValidator : AbstractValidator<UpdateSaleStatusRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleStatusRequest
    /// </summary>
    public UpdateSaleStatusRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required");
    }
} 