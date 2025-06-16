using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for SaleItem entity operations
/// </summary>
public interface ISaleItemRepository
{
    /// <summary>
    /// Creates a new sale item in the repository
    /// </summary>
    /// <param name="saleItem">The sale item to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item</returns>
    Task<SaleItem> CreateAsync(SaleItem saleItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale item by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale item</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item if found, null otherwise</returns>
    Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale item by their unique identifier and sale ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale item</param>
    /// <param name="saleId">The sale ID that the item belongs to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item if found, null otherwise</returns>
    Task<SaleItem?> GetByIdAndSaleIdAsync(Guid id, Guid saleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sale items for a specific sale with pagination
    /// </summary>
    /// <param name="saleId">The sale ID to filter by</param>
    /// <param name="page">Page number (starts from 1)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the list of sale items and total count</returns>
    Task<(IEnumerable<SaleItem> SaleItems, int TotalCount)> GetBySaleIdPagedAsync(
        Guid saleId,
        int page, 
        int size, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all sale items for a specific sale
    /// </summary>
    /// <param name="saleId">The sale ID to filter by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of sale items for the sale</returns>
    Task<IEnumerable<SaleItem>> GetBySaleIdAsync(Guid saleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale item in the repository
    /// </summary>
    /// <param name="saleItem">The sale item to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale item</returns>
    Task<SaleItem> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale item from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the sale item to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale item was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale item from the repository by ID and Sale ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale item to delete</param>
    /// <param name="saleId">The sale ID that the item belongs to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale item was deleted, false if not found</returns>
    Task<bool> DeleteBySaleIdAsync(Guid id, Guid saleId, CancellationToken cancellationToken = default);
} 