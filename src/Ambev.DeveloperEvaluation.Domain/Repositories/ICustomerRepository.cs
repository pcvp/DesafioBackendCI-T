using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Customer entity operations
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Creates a new customer in the repository
    /// </summary>
    /// <param name="customer">The customer to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer</returns>
    Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a customer by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a customer by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves customers with pagination and optional filtering
    /// </summary>
    /// <param name="page">Page number (starts from 1)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="nameFilter">Optional filter by customer name</param>
    /// <param name="emailFilter">Optional filter by customer email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the list of customers and total count</returns>
    Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPagedAsync(
        int page, 
        int size, 
        string? nameFilter = null, 
        string? emailFilter = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing customer in the repository
    /// </summary>
    /// <param name="customer">The customer to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer</returns>
    Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a customer from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the customer was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 