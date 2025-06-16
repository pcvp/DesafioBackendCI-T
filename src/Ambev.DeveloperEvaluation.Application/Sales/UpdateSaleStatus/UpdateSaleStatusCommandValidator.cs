using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleStatus;

/// <summary>
/// Validator for UpdateSaleStatusCommand
/// </summary>
public class UpdateSaleStatusCommandValidator : AbstractValidator<UpdateSaleStatusCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleStatusCommand
    /// </summary>
    public UpdateSaleStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required");
    }
} 