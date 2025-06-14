namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Result model for CreateBranch operation
/// </summary>
public class CreateBranchResult
{
    /// <summary>
    /// The unique identifier of the created branch
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the branch is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The date and time when the branch was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date and time of the last update
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 