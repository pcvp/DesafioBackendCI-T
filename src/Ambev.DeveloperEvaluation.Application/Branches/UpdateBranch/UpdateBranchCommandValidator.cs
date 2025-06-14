using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Validator for UpdateBranchCommand
/// </summary>
public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateBranchCommand
    /// </summary>
    public UpdateBranchCommandValidator()
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