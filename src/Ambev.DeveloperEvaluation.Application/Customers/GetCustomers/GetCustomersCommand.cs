using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

/// <summary>
/// Command for retrieving customers with pagination and filtering.
/// </summary>
public class GetCustomersCommand : IRequest<GetCustomersResult>
{
    /// <summary>
    /// Page number for pagination (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Optional filter by customer name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Optional filter by customer email
    /// </summary>
    public string? Email { get; set; }
} 