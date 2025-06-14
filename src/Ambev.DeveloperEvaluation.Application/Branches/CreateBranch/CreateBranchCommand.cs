using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Command for creating a new branch
/// </summary>
public class CreateBranchCommand : IRequest<CreateBranchResult>
{
    /// <summary>
    /// The branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Validates the command using CreateBranchCommandValidator
    /// </summary>
    /// <returns>Validation result containing any errors found</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CreateBranchCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 