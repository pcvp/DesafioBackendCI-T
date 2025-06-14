using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

/// <summary>
/// Command for retrieving paginated branches
/// </summary>
public class GetBranchesCommand : IRequest<GetBranchesResult>
{
    /// <summary>
    /// Page number (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Optional filter by branch name
    /// </summary>
    public string? Name { get; set; }
} 