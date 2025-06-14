namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Request model for creating a new branch
/// </summary>
public class CreateBranchRequest
{
    /// <summary>
    /// The branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;
} 