using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleItemRepository using Entity Framework Core
/// </summary>
public class SaleItemRepository : ISaleItemRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleItemRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleItemRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale item in the database
    /// </summary>
    public async Task<SaleItem> CreateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        await _context.SaleItems.AddAsync(saleItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return saleItem;
    }

    /// <summary>
    /// Retrieves a sale item by their unique identifier
    /// </summary>
    public async Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItems.FirstOrDefaultAsync(si => si.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale item by their unique identifier and sale ID
    /// </summary>
    public async Task<SaleItem?> GetByIdAndSaleIdAsync(Guid id, Guid saleId, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItems
            .FirstOrDefaultAsync(si => si.Id == id && si.SaleId == saleId, cancellationToken);
    }

    /// <summary>
    /// Retrieves sale items for a specific sale with pagination
    /// </summary>
    public async Task<(IEnumerable<SaleItem> SaleItems, int TotalCount)> GetBySaleIdPagedAsync(
        Guid saleId,
        int page, 
        int size, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.SaleItems.Where(si => si.SaleId == saleId);

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var saleItems = await query
            .OrderBy(si => si.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (saleItems, totalCount);
    }

    /// <summary>
    /// Retrieves all sale items for a specific sale
    /// </summary>
    public async Task<IEnumerable<SaleItem>> GetBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItems
            .Where(si => si.SaleId == saleId)
            .OrderBy(si => si.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale item in the database
    /// </summary>
    public async Task<SaleItem> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        _context.SaleItems.Update(saleItem);
        await _context.SaveChangesAsync(cancellationToken);
        return saleItem;
    }

    /// <summary>
    /// Deletes a sale item from the database
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var saleItem = await GetByIdAsync(id, cancellationToken);
        if (saleItem == null)
            return false;

        _context.SaleItems.Remove(saleItem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Deletes a sale item from the database by ID and Sale ID
    /// </summary>
    public async Task<bool> DeleteBySaleIdAsync(Guid id, Guid saleId, CancellationToken cancellationToken = default)
    {
        var saleItem = await GetByIdAndSaleIdAsync(id, saleId, cancellationToken);
        if (saleItem == null)
            return false;

        _context.SaleItems.Remove(saleItem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 