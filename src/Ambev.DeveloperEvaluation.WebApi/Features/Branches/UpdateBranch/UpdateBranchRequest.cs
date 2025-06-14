namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Request model for updating a branch
/// </summary>
public class UpdateBranchRequest
{
    /// <summary>
    /// The unique identifier of the branch to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The updated branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;
} 