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
    /// Gets or sets the branch ID where the sale was made.
    /// Must be a valid branch reference.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the product ID that was sold.
    /// Must be a valid product reference.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of products sold.
    /// Must be greater than zero.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product at the time of sale.
    /// Must be greater than zero.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to the sale.
    /// Must be between 0 and 100.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this sale item (calculated).
    /// Calculated as: Quantity * UnitPrice * (1 - Discount/100)
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the total sale amount (same as TotalAmount for single item sales).
    /// In future implementations with multiple items, this would be the sum of all items.
    /// </summary>
    public decimal TotalSaleAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale is cancelled.
    /// Cancelled sales cannot be modified.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        IsCancelled = false;
        CalculateTotalAmount();
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
    /// Cancels the sale.
    /// Changes the sale status to Cancelled and updates the timestamp.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is already cancelled</exception>
    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Sale is already cancelled");

        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reactivates a cancelled sale.
    /// Changes the sale status to Active and updates the timestamp.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is not cancelled</exception>
    public void Reactivate()
    {
        if (!IsCancelled)
            throw new InvalidOperationException("Sale is not cancelled");

        IsCancelled = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the sale information.
    /// Recalculates totals and updates the timestamp.
    /// </summary>
    /// <param name="saleNumber">The new sale number</param>
    /// <param name="saleDate">The new sale date</param>
    /// <param name="customerId">The new customer ID</param>
    /// <param name="branchId">The new branch ID</param>
    /// <param name="productId">The new product ID</param>
    /// <param name="quantity">The new quantity</param>
    /// <param name="unitPrice">The new unit price</param>
    /// <param name="discount">The new discount percentage</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to update a cancelled sale</exception>
    public void UpdateSaleInfo(string saleNumber, DateTime saleDate, Guid customerId, Guid branchId, 
        Guid productId, int quantity, decimal unitPrice, decimal discount)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot update a cancelled sale");

        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount based on quantity, unit price, and discount.
    /// </summary>
    private void CalculateTotalAmount()
    {
        TotalAmount = Quantity * UnitPrice * (1 - Discount / 100);
        TotalSaleAmount = TotalAmount; // For single item sales, both are the same
    }

    /// <summary>
    /// Applies a discount to the sale.
    /// </summary>
    /// <param name="discountPercentage">The discount percentage to apply (0-100)</param>
    /// <exception cref="ArgumentException">Thrown when discount is invalid</exception>
    /// <exception cref="InvalidOperationException">Thrown when trying to apply discount to a cancelled sale</exception>
    public void ApplyDiscount(decimal discountPercentage)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot apply discount to a cancelled sale");

        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount must be between 0 and 100 percent", nameof(discountPercentage));

        Discount = discountPercentage;
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }
} 