using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Validator for CreateBranchCommand
/// </summary>
public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    /// <summary>
    /// Initializes validation rules for CreateBranchCommand
    /// </summary>
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .Length(2, 100)
            .WithMessage("Branch name must be between 2 and 100 characters");
    }
} 