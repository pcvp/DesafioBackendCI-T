namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;

/// <summary>
/// Response model for GetBranches operation
/// </summary>
public class GetBranchesResponse
{
    /// <summary>
    /// List of branches
    /// </summary>
    public List<BranchSummary> Data { get; set; } = [];

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Total number of branches
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Indicates if there are more pages
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;

    /// <summary>
    /// Indicates if there are previous pages
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;
}

/// <summary>
/// Summary information for a branch in the list
/// </summary>
public class BranchSummary
{
    /// <summary>
    /// The unique identifier of the branch
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
} 