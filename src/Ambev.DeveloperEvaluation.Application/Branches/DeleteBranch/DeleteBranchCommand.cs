using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Command for deleting a branch
/// </summary>
public class DeleteBranchCommand : IRequest
{
    /// <summary>
    /// The unique identifier of the branch to delete
    /// </summary>
    public Guid Id { get; set; }
} 