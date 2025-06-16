namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Response model for sale creation
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID where the sale was made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the sale status
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Gets or sets the total amount for the sale (sum of all items)
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the list of items in this sale
    /// </summary>
    public List<CreateSaleItemResponse> Items { get; set; } = new List<CreateSaleItemResponse>();

    /// <summary>
    /// Gets or sets the date and time when the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Response model for sale item within a sale creation response
/// </summary>
public class CreateSaleItemResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of products
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to this item
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this sale item
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
} 