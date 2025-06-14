using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover business methods, status changes, and validation scenarios.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that when an active sale is cancelled, their status changes to Cancelled.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to Cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.IsCancelled = false; // Ensure sale is active

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that when a cancelled sale is reactivated, their status changes to Active.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to Active when reactivated")]
    public void Given_CancelledSale_When_Reactivated_Then_StatusShouldBeActive()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();
        Assert.True(sale.IsCancelled); // Ensure sale is cancelled

        // Act
        sale.Reactivate();

        // Assert
        Assert.False(sale.IsCancelled);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that cancelling an already cancelled sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Cancelling already cancelled sale should throw exception")]
    public void Given_CancelledSale_When_CancelledAgain_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.Cancel());
    }

    /// <summary>
    /// Tests that reactivating an active sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Reactivating active sale should throw exception")]
    public void Given_ActiveSale_When_Reactivated_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.IsCancelled = false; // Ensure sale is active

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.Reactivate());
    }

    /// <summary>
    /// Tests that UpdateSaleInfo method updates all sale information correctly.
    /// </summary>
    [Fact(DisplayName = "UpdateSaleInfo should update all sale information")]
    public void Given_ValidSaleInfo_When_UpdateSaleInfo_Then_ShouldUpdateAllFields()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var newSaleNumber = SaleTestData.GenerateValidSaleNumber();
        var newSaleDate = SaleTestData.GenerateValidSaleDate();
        var newCustomerId = Guid.NewGuid();
        var newBranchId = Guid.NewGuid();
        var newProductId = Guid.NewGuid();
        var newQuantity = SaleTestData.GenerateValidQuantity();
        var newUnitPrice = SaleTestData.GenerateValidUnitPrice();
        var newDiscount = SaleTestData.GenerateValidDiscount();

        // Act
        sale.UpdateSaleInfo(newSaleNumber, newSaleDate, newCustomerId, newBranchId, 
            newProductId, newQuantity, newUnitPrice, newDiscount);

        // Assert
        Assert.Equal(newSaleNumber, sale.SaleNumber);
        Assert.Equal(newSaleDate, sale.SaleDate);
        Assert.Equal(newCustomerId, sale.CustomerId);
        Assert.Equal(newBranchId, sale.BranchId);
        Assert.Equal(newProductId, sale.ProductId);
        Assert.Equal(newQuantity, sale.Quantity);
        Assert.Equal(newUnitPrice, sale.UnitPrice);
        Assert.Equal(newDiscount, sale.Discount);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that updating cancelled sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Updating cancelled sale should throw exception")]
    public void Given_CancelledSale_When_UpdateSaleInfo_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            sale.UpdateSaleInfo("NEW001", DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), 
                Guid.NewGuid(), 1, 10m, 0m));
    }

    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes for minimal sale data.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for minimal sale data")]
    public void Given_MinimalSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateMinimalSale();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale number is empty.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when sale number is empty")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleNumber = string.Empty;

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when quantity is zero or negative.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when quantity is invalid")]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Quantity = SaleTestData.GenerateInvalidQuantity();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when unit price is negative.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when unit price is negative")]
    public void Given_NegativeUnitPrice_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.UnitPrice = SaleTestData.GenerateInvalidUnitPrice();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when discount is invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when discount is invalid")]
    public void Given_InvalidDiscount_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Discount = SaleTestData.GenerateInvalidDiscount();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that ApplyDiscount method applies discount correctly.
    /// </summary>
    [Fact(DisplayName = "ApplyDiscount should apply discount correctly")]
    public void Given_ValidDiscount_When_ApplyDiscount_Then_ShouldApplyCorrectly()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Quantity = 2;
        sale.UnitPrice = 100m;
        sale.Discount = 0m;
        var discountToApply = 10m; // 10%

        // Act
        sale.ApplyDiscount(discountToApply);

        // Assert
        Assert.Equal(discountToApply, sale.Discount);
        Assert.Equal(180m, sale.TotalAmount); // 2 * 100 * (1 - 0.1) = 180
        Assert.Equal(180m, sale.TotalSaleAmount);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that applying discount to cancelled sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Applying discount to cancelled sale should throw exception")]
    public void Given_CancelledSale_When_ApplyDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.ApplyDiscount(10m));
    }

    /// <summary>
    /// Tests that applying invalid discount throws exception.
    /// </summary>
    [Fact(DisplayName = "Applying invalid discount should throw exception")]
    public void Given_InvalidDiscountValue_When_ApplyDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sale.ApplyDiscount(-5m)); // Negative discount
        Assert.Throws<ArgumentException>(() => sale.ApplyDiscount(150m)); // Over 100% discount
    }

    /// <summary>
    /// Tests that sale is created with default values.
    /// </summary>
    [Fact(DisplayName = "Sale should be created with default values")]
    public void Given_NewSale_When_Created_Then_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var sale = new Sale();

        // Assert
        Assert.False(sale.IsCancelled);
        Assert.True(sale.CreatedAt <= DateTime.UtcNow);
        Assert.Null(sale.UpdatedAt);
        Assert.Equal(0m, sale.TotalAmount);
        Assert.Equal(0m, sale.TotalSaleAmount);
    }

    /// <summary>
    /// Tests that total amount is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Total amount should be calculated correctly")]
    public void Given_SaleWithValues_When_CalculatingTotal_Then_ShouldCalculateCorrectly()
    {
        // Arrange
        var sale = new Sale
        {
            Quantity = 3,
            UnitPrice = 50m,
            Discount = 20m // 20%
        };

        // Act
        sale.UpdateSaleInfo(sale.SaleNumber, sale.SaleDate, sale.CustomerId, sale.BranchId,
            sale.ProductId, sale.Quantity, sale.UnitPrice, sale.Discount);

        // Assert
        Assert.Equal(120m, sale.TotalAmount); // 3 * 50 * (1 - 0.2) = 120
        Assert.Equal(120m, sale.TotalSaleAmount);
    }
} 