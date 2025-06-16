using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system with all transaction information.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number.
    /// Must not be null or empty and should be unique.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// Cannot be in the future.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer ID who made the purchase.
    /// Must be a valid customer reference.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer who made the purchase.
    /// Must be a valid customer reference.
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// Gets or sets the branch ID where the sale was made.
    /// Must be a valid branch reference.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch where the sale was made.
    /// Must be a valid branch reference.
    /// </summary>
    public Branch Branch { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this sale (calculated).
    /// Calculated as the sum of all sale items' total amounts.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the sale items.
    /// A sale can have multiple items.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new List<SaleItem>();

    /// <summary>
    /// Gets or sets the sale status.
    /// </summary>
    public SaleStatusEnum Status { get; set; }

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }



    /// <summary>
    /// Gets the total sale amount (same as TotalAmount for compatibility).
    /// </summary>
    public decimal TotalSaleAmount => TotalAmount;

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        Status = SaleStatusEnum.Pending;
        Items = new List<SaleItem>();
        CalculateTotalAmount();
    }

    /// <summary>
    /// Initializes a new instance of the Sale class with basic information.
    /// </summary>
    /// <param name="saleNumber">The sale number</param>
    /// <param name="saleDate">The sale date</param>
    /// <param name="customerId">The customer ID</param>
    /// <param name="branchId">The branch ID</param>
    public Sale(string saleNumber, DateTime saleDate, Guid customerId, Guid branchId) : this()
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
    }

    /// <summary>
    /// Performs validation of the sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Adds a sale item to the sale.
    /// </summary>
    /// <param name="saleItem">The sale item to add</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to add item to a cancelled or closed sale</exception>
    /// <exception cref="ArgumentNullException">Thrown when sale item is null</exception>
    public void AddItem(SaleItem saleItem)
    {
        if (Status != SaleStatusEnum.Pending)
            throw new InvalidOperationException("Cannot add items to a cancelled or closed sale");

        if (saleItem == null)
            throw new ArgumentNullException(nameof(saleItem));

        Items.Add(saleItem);
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a sale item from the sale.
    /// </summary>
    /// <param name="saleItem">The sale item to remove</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to remove item from a cancelled or closed sale</exception>
    /// <exception cref="ArgumentNullException">Thrown when sale item is null</exception>
    public void RemoveItem(SaleItem saleItem)
    {
        if (Status != SaleStatusEnum.Pending)
            throw new InvalidOperationException("Cannot remove items from a cancelled or closed sale");

        if (saleItem == null)
            throw new ArgumentNullException(nameof(saleItem));

        Items.Remove(saleItem);
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the sale.
    /// Changes the sale status to Cancelled and updates the timestamp.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is already cancelled</exception>
    public void Cancel()
    {
        if (Status == SaleStatusEnum.Cancelled)
            throw new InvalidOperationException("Sale is already cancelled");

        Status = SaleStatusEnum.Cancelled;
        
        // Cancel all items
        foreach (var item in Items.Where(i => !i.IsCancelled))
        {
            item.Cancel();
        }
        
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Pays the sale.
    /// Changes the sale status to Pay and updates the timestamp.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is already cancelled</exception>
    public void Pay()
    {
        if (Status == SaleStatusEnum.Paid)
            throw new InvalidOperationException("Sale is already payed");

        Status = SaleStatusEnum.Paid;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Close the sale.
    /// Changes the sale status to Closed and updates the timestamp.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is already cancelled</exception>
    public void Close()
    {
        if (Status == SaleStatusEnum.Cancelled)
            throw new InvalidOperationException("Cannot close a cancelled sale");

        if (Status == SaleStatusEnum.Closed)
            throw new InvalidOperationException("Sale is already closed");

        Status = SaleStatusEnum.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reactivates a cancelled sale.
    /// Changes the sale status to Pending and updates the timestamp.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is not cancelled</exception>
    public void Reactivate()
    {
        if (Status != SaleStatusEnum.Cancelled)
            throw new InvalidOperationException("Sale is not cancelled");

        Status = SaleStatusEnum.Pending;
        
        // Reactivate all cancelled items
        foreach (var item in Items.Where(i => i.IsCancelled))
        {
            item.Reactivate();
        }
        
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the sale basic information.
    /// </summary>
    /// <param name="saleNumber">The new sale number</param>
    /// <param name="saleDate">The new sale date</param>
    /// <param name="customerId">The new customer ID</param>
    /// <param name="branchId">The new branch ID</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to update a cancelled or closed sale</exception>
    public void UpdateSaleInfo(string saleNumber, DateTime saleDate, Guid customerId, Guid branchId)
    {
        if (Status != SaleStatusEnum.Pending)
            throw new InvalidOperationException("Cannot update a cancelled or closed sale");

        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
        
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount based on all sale items.
    /// </summary>
    private void CalculateTotalAmount()
    {
        TotalAmount = Items?.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount) ?? 0;
    }

    /// <summary>
    /// Applies a discount to all items in the sale.
    /// </summary>
    /// <param name="discountPercentage">The discount percentage to apply (0-100)</param>
    /// <exception cref="ArgumentException">Thrown when discount is invalid</exception>
    /// <exception cref="InvalidOperationException">Thrown when trying to apply discount to a cancelled or closed sale</exception>
    public void ApplyDiscountToAllItems(decimal discountPercentage)
    {
        if (Status != SaleStatusEnum.Pending)
            throw new InvalidOperationException("Cannot apply discount to a cancelled or closed sale");

        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount must be between 0 and 100 percent", nameof(discountPercentage));

        foreach (var item in Items.Where(i => !i.IsCancelled))
        {
            item.ApplyDiscount(discountPercentage);
        }

        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the total quantity of all items in the sale.
    /// </summary>
    /// <returns>The total quantity</returns>
    public int GetTotalQuantity()
    {
        return Items?.Where(i => !i.IsCancelled).Sum(i => i.Quantity) ?? 0;
    }

    /// <summary>
    /// Gets all active (non-cancelled) items in the sale.
    /// </summary>
    /// <returns>List of active sale items</returns>
    public List<SaleItem> GetActiveItems()
    {
        return Items?.Where(i => !i.IsCancelled).ToList() ?? new List<SaleItem>();
    }
} 

public enum SaleStatusEnum
{
    Pending = 0,
    Closed = 1,
    Paid = 2,
    Cancelled = 3
}