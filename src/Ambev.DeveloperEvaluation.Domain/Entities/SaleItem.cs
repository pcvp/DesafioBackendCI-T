using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale item in the system with product information and business rules.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the sale ID that this item belongs to.
    /// Must not be empty.
    /// </summary>
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Gets the sale that this item belongs to.
    /// Must not be empty.
    /// </summary>
    public Sale? Sale { get; private set; }

    /// <summary>
    /// Gets the product ID for this sale item.
    /// Must not be empty.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the product for this sale item.
    /// Must not be empty.
    /// </summary>
    public Product Product { get; private set; }

    /// <summary>
    /// Gets the quantity of the product.
    /// Must be greater than 0 and cannot exceed 20 items per sale item.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// Must be greater than 0 and cannot exceed $10,000.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount percentage applied to this item (0-100).
    /// Cannot be negative and cannot exceed 100%.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item (calculated).
    /// Calculated as: Quantity * UnitPrice * (1 - Discount/100)
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets whether this item is cancelled.
    /// Cancelled items cannot be modified.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets the date and time when the item was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the item was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
        CreatedAt = DateTime.UtcNow;
        IsCancelled = false;
    }

    /// <summary>
    /// Initializes a new instance of the SaleItem class with specified values.
    /// </summary>
    /// <param name="saleId">The sale ID that this item belongs to</param>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The quantity of the product</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <param name="discount">The discount percentage (0-100)</param>
    public SaleItem(Guid saleId, Guid productId, int quantity, decimal unitPrice, decimal discount = 0)
        : this()
    {
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        
        CalculateTotalAmount();
        // Don't set UpdatedAt on initial creation - only on updates
    }

    /// <summary>
    /// Performs validation of the sale item entity using the SaleItemValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Cancels the sale item.
    /// Changes the item's status to Cancelled and prevents further modifications.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the item is already cancelled</exception>
    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Sale item is already cancelled");

        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reactivates the sale item.
    /// Changes the item's status to Active, allowing modifications.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the item is not cancelled</exception>
    public void Reactivate()
    {
        if (!IsCancelled)
            throw new InvalidOperationException("Sale item is not cancelled");

        IsCancelled = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the sale item information.
    /// Recalculates the total amount based on new values.
    /// </summary>
    /// <param name="saleId">The sale ID that this item belongs to</param>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The quantity of the product</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <param name="discount">The discount percentage (0-100)</param>
    /// <exception cref="InvalidOperationException">Thrown when the item is cancelled</exception>
    public void UpdateItemInfo(Guid saleId, Guid productId, int quantity, decimal unitPrice, decimal discount = 0)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot update a cancelled sale item");

        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Applies a discount to the sale item.
    /// Recalculates the total amount with the new discount.
    /// </summary>
    /// <param name="discountPercentage">The discount percentage to apply (0-100)</param>
    /// <exception cref="InvalidOperationException">Thrown when the item is cancelled</exception>
    /// <exception cref="ArgumentException">Thrown when discount is invalid</exception>
    public void ApplyDiscount(decimal discountPercentage)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot apply discount to a cancelled sale item");

        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));

        Discount = discountPercentage;
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount for this sale item.
    /// Formula: Quantity * UnitPrice * (1 - Discount/100)
    /// </summary>
    private void CalculateTotalAmount()
    {
        TotalAmount = Quantity * UnitPrice * (1 - Discount / 100);
    }
} 