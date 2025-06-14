using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Command for updating a branch
/// </summary>
public class UpdateBranchCommand : IRequest<UpdateBranchResult>
{
    /// <summary>
    /// The unique identifier of the branch to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The updated branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Validates the command using UpdateBranchCommandValidator
    /// </summary>
    /// <returns>Validation result containing any errors found</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new UpdateBranchCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 