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
    /// Tests that when a sale is cancelled, their status changes to Cancelled.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to Cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.Status == Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Cancelled);
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

        // Act
        sale.Reactivate();

        // Assert
        Assert.False(sale.Status == Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Cancelled);
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

        // Act
        sale.UpdateSaleInfo(newSaleNumber, newSaleDate, newCustomerId, newBranchId);

        // Assert
        Assert.Equal(newSaleNumber, sale.SaleNumber);
        Assert.Equal(newSaleDate, sale.SaleDate);
        Assert.Equal(newCustomerId, sale.CustomerId);
        Assert.Equal(newBranchId, sale.BranchId);
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
            sale.UpdateSaleInfo("NEW001", DateTime.Now, Guid.NewGuid(), Guid.NewGuid()));
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
    /// Tests that sale is created with default values.
    /// </summary>
    [Fact(DisplayName = "Sale should be created with default values")]
    public void Given_NewSale_When_Created_Then_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var sale = new Sale();

        // Assert
        Assert.Equal(Guid.Empty, sale.Id);
        Assert.True(string.IsNullOrEmpty(sale.SaleNumber));
        Assert.Equal(DateTime.MinValue, sale.SaleDate);
        Assert.Equal(Guid.Empty, sale.CustomerId);
        Assert.Equal(Guid.Empty, sale.BranchId);
        Assert.False(sale.Status == Ambev.DeveloperEvaluation.Domain.Entities.SaleStatusEnum.Cancelled); // Default should be active
        Assert.NotEqual(DateTime.MinValue, sale.CreatedAt); // Should be set in constructor
        Assert.Null(sale.UpdatedAt);
    }
} 