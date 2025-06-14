using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Validator for CreateBranchRequest
/// </summary>
public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateBranchRequest
    /// </summary>
    public CreateBranchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .Length(2, 100)
            .WithMessage("Branch name must be between 2 and 100 characters");
    }
} 