using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICustomerRepository using Entity Framework Core
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CustomerRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CustomerRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new customer in the database
    /// </summary>
    /// <param name="customer">The customer to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer</returns>
    public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    /// <summary>
    /// Retrieves a customer by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a customer by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    /// <summary>
    /// Retrieves customers with pagination and optional filtering
    /// </summary>
    /// <param name="page">Page number (starts from 1)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="nameFilter">Optional filter by customer name</param>
    /// <param name="emailFilter">Optional filter by customer email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the list of customers and total count</returns>
    public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPagedAsync(
        int page, 
        int size, 
        string? nameFilter = null, 
        string? emailFilter = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Customers.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(c => c.Name.Contains(nameFilter));
        }

        if (!string.IsNullOrEmpty(emailFilter))
        {
            query = query.Where(c => c.Email != null && c.Email.Contains(emailFilter));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var customers = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (customers, totalCount);
    }

    /// <summary>
    /// Updates an existing customer in the database
    /// </summary>
    /// <param name="customer">The customer to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer</returns>
    public async Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    /// <summary>
    /// Deletes a customer from the database
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the customer was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 