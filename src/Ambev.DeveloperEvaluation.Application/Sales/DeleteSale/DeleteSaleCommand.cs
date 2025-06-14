using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for deleting a sale
/// </summary>
public class DeleteSaleCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to delete
    /// </summary>
    public Guid Id { get; set; }
} 