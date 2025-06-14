using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;

/// <summary>
/// Validator for GetBranchesRequest
/// </summary>
public class GetBranchesRequestValidator : AbstractValidator<GetBranchesRequest>
{
    /// <summary>
    /// Initializes validation rules for GetBranchesRequest
    /// </summary>
    public GetBranchesRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Size cannot exceed 100");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name filter cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));
    }
} 