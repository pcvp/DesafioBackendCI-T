using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover business methods, status changes, and validation scenarios.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that when an active sale item is cancelled, their status changes to Cancelled.
    /// </summary>
    [Fact(DisplayName = "SaleItem status should change to Cancelled when cancelled")]
    public void Given_ActiveSaleItem_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        Assert.False(saleItem.IsCancelled);

        // Act
        saleItem.Cancel();

        // Assert
        Assert.True(saleItem.IsCancelled);
        Assert.NotNull(saleItem.UpdatedAt);
    }

    /// <summary>
    /// Tests that when a cancelled sale item is reactivated, their status changes to Active.
    /// </summary>
    [Fact(DisplayName = "SaleItem status should change to Active when reactivated")]
    public void Given_CancelledSaleItem_When_Reactivated_Then_StatusShouldBeActive()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Cancel();
        Assert.True(saleItem.IsCancelled);

        // Act
        saleItem.Reactivate();

        // Assert
        Assert.False(saleItem.IsCancelled);
        Assert.NotNull(saleItem.UpdatedAt);
    }

    /// <summary>
    /// Tests that cancelling an already cancelled sale item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when trying to cancel an already cancelled sale item")]
    public void Given_CancelledSaleItem_When_CancelledAgain_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Cancel();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => saleItem.Cancel());
        Assert.Equal("Sale item is already cancelled", exception.Message);
    }

    /// <summary>
    /// Tests that reactivating an active sale item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when trying to reactivate an active sale item")]
    public void Given_ActiveSaleItem_When_Reactivated_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        Assert.False(saleItem.IsCancelled);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => saleItem.Reactivate());
        Assert.Equal("Sale item is not cancelled", exception.Message);
    }

    /// <summary>
    /// Tests that validation passes when all sale item properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale item data")]
    public void Given_ValidSaleItemData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that UpdateItemInfo updates all properties correctly.
    /// </summary>
    [Fact(DisplayName = "UpdateItemInfo should update all properties and recalculate total")]
    public void Given_SaleItem_When_UpdateItemInfo_Then_ShouldUpdatePropertiesAndRecalculateTotal()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var newSaleId = Guid.NewGuid();
        var newProductId = Guid.NewGuid();
        var newQuantity = 5;
        var newUnitPrice = 100m;
        var newDiscount = 10m;
        var expectedTotal = newQuantity * newUnitPrice * (1 - newDiscount / 100); // 5 * 100 * 0.9 = 450

        // Act
        saleItem.UpdateItemInfo(newSaleId, newProductId, newQuantity, newUnitPrice, newDiscount);

        // Assert
        Assert.Equal(newSaleId, saleItem.SaleId);
        Assert.Equal(newProductId, saleItem.ProductId);
        Assert.Equal(newQuantity, saleItem.Quantity);
        Assert.Equal(newUnitPrice, saleItem.UnitPrice);
        Assert.Equal(newDiscount, saleItem.Discount);
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
        Assert.NotNull(saleItem.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdateItemInfo throws exception when sale item is cancelled.
    /// </summary>
    [Fact(DisplayName = "UpdateItemInfo should throw exception when sale item is cancelled")]
    public void Given_CancelledSaleItem_When_UpdateItemInfo_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Cancel();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            saleItem.UpdateItemInfo(Guid.NewGuid(), Guid.NewGuid(), 1, 10m, 0m));
        Assert.Equal("Cannot update a cancelled sale item", exception.Message);
    }

    /// <summary>
    /// Tests that ApplyDiscount updates discount and recalculates total.
    /// </summary>
    [Fact(DisplayName = "ApplyDiscount should update discount and recalculate total")]
    public void Given_SaleItem_When_ApplyDiscount_Then_ShouldUpdateDiscountAndRecalculateTotal()
    {
        // Arrange
        var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 100m, 0m);
        var originalTotal = saleItem.TotalAmount; // 200
        var newDiscount = 20m;
        var expectedTotal = 2 * 100m * (1 - newDiscount / 100); // 2 * 100 * 0.8 = 160

        // Act
        saleItem.ApplyDiscount(newDiscount);

        // Assert
        Assert.Equal(newDiscount, saleItem.Discount);
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
        Assert.NotEqual(originalTotal, saleItem.TotalAmount);
        Assert.NotNull(saleItem.UpdatedAt);
    }

    /// <summary>
    /// Tests that ApplyDiscount throws exception when sale item is cancelled.
    /// </summary>
    [Fact(DisplayName = "ApplyDiscount should throw exception when sale item is cancelled")]
    public void Given_CancelledSaleItem_When_ApplyDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Cancel();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => saleItem.ApplyDiscount(10m));
        Assert.Equal("Cannot apply discount to a cancelled sale item", exception.Message);
    }

    /// <summary>
    /// Tests that ApplyDiscount throws exception for negative discount.
    /// </summary>
    [Fact(DisplayName = "ApplyDiscount should throw exception for negative discount")]
    public void Given_SaleItem_When_ApplyNegativeDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => saleItem.ApplyDiscount(-5m));
        Assert.Equal("Discount percentage must be between 0 and 100 (Parameter 'discountPercentage')", exception.Message);
    }

    /// <summary>
    /// Tests that ApplyDiscount throws exception for discount over 100%.
    /// </summary>
    [Fact(DisplayName = "ApplyDiscount should throw exception for discount over 100%")]
    public void Given_SaleItem_When_ApplyExcessiveDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => saleItem.ApplyDiscount(150m));
        Assert.Equal("Discount percentage must be between 0 and 100 (Parameter 'discountPercentage')", exception.Message);
    }

    /// <summary>
    /// Tests that sale item is created with default values.
    /// </summary>
    [Fact(DisplayName = "SaleItem should be created with default values")]
    public void Given_NewSaleItem_When_Created_Then_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var saleItem = new SaleItem();

        // Assert
        Assert.Equal(Guid.Empty, saleItem.Id);
        Assert.Equal(Guid.Empty, saleItem.SaleId);
        Assert.Equal(Guid.Empty, saleItem.ProductId);
        Assert.Equal(0, saleItem.Quantity);
        Assert.Equal(0m, saleItem.UnitPrice);
        Assert.Equal(0m, saleItem.Discount);
        Assert.Equal(0m, saleItem.TotalAmount);
        Assert.False(saleItem.IsCancelled); // Default should be active
        Assert.NotEqual(DateTime.MinValue, saleItem.CreatedAt); // Should be set in constructor
        Assert.Null(saleItem.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdatedAt is set when sale item is cancelled.
    /// </summary>
    [Fact(DisplayName = "UpdatedAt should be set when sale item is cancelled")]
    public void Given_SaleItem_When_Cancelled_Then_UpdatedAtShouldBeSet()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var originalUpdatedAt = saleItem.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(1); // Ensure time difference
        saleItem.Cancel();

        // Assert
        Assert.NotNull(saleItem.UpdatedAt);
        Assert.NotEqual(originalUpdatedAt, saleItem.UpdatedAt);
    }

    /// <summary>
    /// Tests that total amount is calculated correctly with different scenarios.
    /// </summary>
    [Theory(DisplayName = "Total amount should be calculated correctly")]
    [InlineData(1, 10.00, 0, 10.00)]     // No discount
    [InlineData(2, 10.00, 0, 20.00)]     // Multiple quantity, no discount
    [InlineData(1, 10.00, 10, 9.00)]     // 10% discount
    [InlineData(2, 10.00, 50, 10.00)]    // 50% discount
    [InlineData(3, 100.00, 25, 225.00)]  // 25% discount
    public void Given_SaleItemValues_When_Created_Then_TotalAmountShouldBeCalculatedCorrectly(
        int quantity, decimal unitPrice, decimal discount, decimal expectedTotal)
    {
        // Arrange & Act
        var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), quantity, unitPrice, discount);

        // Assert
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
    }

    /// <summary>
    /// Tests that constructor sets all properties correctly.
    /// </summary>
    [Fact(DisplayName = "Constructor should set all properties correctly")]
    public void Given_SaleItemParameters_When_CreatedWithConstructor_Then_ShouldSetAllProperties()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var quantity = 5;
        var unitPrice = 50m;
        var discount = 15m;

        // Act
        var saleItem = new SaleItem(saleId, productId, quantity, unitPrice, discount);

        // Assert
        Assert.Equal(saleId, saleItem.SaleId);
        Assert.Equal(productId, saleItem.ProductId);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(discount, saleItem.Discount);
        Assert.False(saleItem.IsCancelled);
        Assert.NotEqual(DateTime.MinValue, saleItem.CreatedAt);
        Assert.Null(saleItem.UpdatedAt);
        // Total should be: 5 * 50 * (1 - 15/100) = 5 * 50 * 0.85 = 212.5
        Assert.Equal(212.5m, saleItem.TotalAmount);
    }
} 