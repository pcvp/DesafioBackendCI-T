namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItems;

/// <summary>
/// Response model for retrieving sale items with pagination
/// </summary>
public class GetSaleItemsResponse
{
    /// <summary>
    /// Gets or sets the list of sale items
    /// </summary>
    public List<SaleItemDto> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of sale items
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets whether there is a next page
    /// </summary>
    public bool HasNext { get; set; }

    /// <summary>
    /// Gets or sets whether there is a previous page
    /// </summary>
    public bool HasPrevious { get; set; }
}

/// <summary>
/// DTO for sale item information in the list
/// </summary>
public class SaleItemDto
{
    /// <summary>
    /// Gets or sets the sale item ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale ID that this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to this item (0-100)
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (calculated)
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the item was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the item was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 