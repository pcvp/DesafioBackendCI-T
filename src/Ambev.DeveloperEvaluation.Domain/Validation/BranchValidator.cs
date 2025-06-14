using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Branch entity that defines validation rules for branch properties.
/// </summary>
public class BranchValidator : AbstractValidator<Branch>
{
    /// <summary>
    /// Initializes a new instance of BranchValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Name: Required, must be between 2 and 100 characters
    /// </remarks>
    public BranchValidator()
    {
        RuleFor(branch => branch.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .Length(2, 100)
            .WithMessage("Branch name must be between 2 and 100 characters");
    }
} 