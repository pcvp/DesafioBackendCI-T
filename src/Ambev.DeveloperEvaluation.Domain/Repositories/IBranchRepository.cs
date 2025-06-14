using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity operations
/// </summary>
public interface IBranchRepository
{
    /// <summary>
    /// Creates a new branch in the repository
    /// </summary>
    /// <param name="branch">The branch to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch</returns>
    Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a branch by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves branches with pagination and optional filtering
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Page size</param>
    /// <param name="nameFilter">Optional filter by branch name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the list of branches and total count</returns>
    Task<(IEnumerable<Branch> Branches, int TotalCount)> GetPagedAsync(
        int page = 1, 
        int size = 10, 
        string? nameFilter = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of branches with optional filtering
    /// </summary>
    /// <param name="nameFilter">Optional filter by branch name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total count of branches</returns>
    Task<int> GetCountAsync(string? nameFilter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing branch in the repository
    /// </summary>
    /// <param name="branch">The branch to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated branch</returns>
    Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a branch from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the branch to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 