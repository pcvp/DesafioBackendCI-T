using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Validator for UpdateBranchRequest
/// </summary>
public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateBranchRequest
    /// </summary>
    public UpdateBranchRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .Length(2, 100)
            .WithMessage("Branch name must be between 2 and 100 characters");
    }
} 