using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Command for retrieving a branch by ID
/// </summary>
public class GetBranchCommand : IRequest<GetBranchResult>
{
    /// <summary>
    /// The unique identifier of the branch to retrieve
    /// </summary>
    public Guid Id { get; set; }
} 