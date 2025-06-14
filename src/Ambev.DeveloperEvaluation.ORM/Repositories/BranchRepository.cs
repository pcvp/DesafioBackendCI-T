using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IBranchRepository using Entity Framework Core
/// </summary>
public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of BranchRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public BranchRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new branch in the database
    /// </summary>
    /// <param name="branch">The branch to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch</returns>
    public async Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Add(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }

    /// <summary>
    /// Retrieves a branch by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Branches.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves branches with pagination and optional filtering
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Page size</param>
    /// <param name="nameFilter">Optional filter by branch name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the list of branches and total count</returns>
    public async Task<(IEnumerable<Branch> Branches, int TotalCount)> GetPagedAsync(
        int page = 1, 
        int size = 10, 
        string? nameFilter = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Branches.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(b => b.Name.Contains(nameFilter));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and ordering
        var branches = await query
            .OrderBy(b => b.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (branches, totalCount);
    }

    /// <summary>
    /// Gets the total count of branches with optional filtering
    /// </summary>
    /// <param name="nameFilter">Optional filter by branch name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total count of branches</returns>
    public async Task<int> GetCountAsync(string? nameFilter = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Branches.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(b => b.Name.Contains(nameFilter));
        }

        return await query.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing branch in the database
    /// </summary>
    /// <param name="branch">The branch to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated branch</returns>
    public async Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }

    /// <summary>
    /// Deletes a branch from the database
    /// </summary>
    /// <param name="id">The unique identifier of the branch to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var branch = await GetByIdAsync(id, cancellationToken);
        if (branch == null)
            return false;

        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 