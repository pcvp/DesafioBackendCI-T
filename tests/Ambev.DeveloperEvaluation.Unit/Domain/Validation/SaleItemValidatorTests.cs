using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleItemValidator class.
/// Tests cover validation scenarios for sale item properties.
/// </summary>
public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    /// <summary>
    /// Tests that validation passes when all sale item properties are valid.
    /// </summary>
    [Fact(DisplayName = "Should pass validation when all sale item properties are valid")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when SaleId is empty.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when SaleId is empty")]
    public void Given_EmptySaleId_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem(
            Guid.Empty, // Empty SaleId
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            SaleItemTestData.GenerateValidUnitPrice(),
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "SaleId" && e.ErrorCode == "NotEmptyValidator");
    }

    /// <summary>
    /// Tests that validation fails when ProductId is empty.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when ProductId is empty")]
    public void Given_EmptyProductId_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem(
            Guid.NewGuid(),
            Guid.Empty, // Empty ProductId
            SaleItemTestData.GenerateValidQuantity(),
            SaleItemTestData.GenerateValidUnitPrice(),
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "ProductId" && e.ErrorCode == "NotEmptyValidator");
    }

    /// <summary>
    /// Tests that validation passes for ProductId at minimum length boundary.
    /// </summary>
    [Fact(DisplayName = "Should pass validation for valid ProductId")]
    public void Given_ValidProductId_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = new SaleItem(
            Guid.NewGuid(),
            Guid.NewGuid(), // Valid ProductId
            SaleItemTestData.GenerateValidQuantity(),
            SaleItemTestData.GenerateValidUnitPrice(),
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when Quantity is zero or negative.
    /// </summary>
    [Theory(DisplayName = "Should fail validation when Quantity is zero or negative")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldReturnInvalid(int invalidQuantity)
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            invalidQuantity,
            SaleItemTestData.GenerateValidUnitPrice(),
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Quantity" && e.ErrorCode == "GreaterThanValidator");
    }

    /// <summary>
    /// Tests that validation fails when Quantity exceeds maximum limit.
    /// </summary>
    [Theory(DisplayName = "Should fail validation when Quantity exceeds maximum limit")]
    [InlineData(21)]
    [InlineData(50)]
    [InlineData(100)]
    public void Given_ExcessiveQuantity_When_Validated_Then_ShouldReturnInvalid(int excessiveQuantity)
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            excessiveQuantity,
            SaleItemTestData.GenerateValidUnitPrice(),
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Quantity" && e.ErrorCode == "LessThanOrEqualValidator");
    }

    /// <summary>
    /// Tests that validation passes for valid Quantity values.
    /// </summary>
    [Theory(DisplayName = "Should pass validation for valid Quantity values")]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void Given_ValidQuantity_When_Validated_Then_ShouldReturnValid(int validQuantity)
    {
        // Arrange
        var saleItem = new SaleItem(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            validQuantity,
            SaleItemTestData.GenerateValidUnitPrice(),
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when UnitPrice is zero or negative.
    /// </summary>
    [Theory(DisplayName = "Should fail validation when UnitPrice is zero or negative")]
    [InlineData(0)]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void Given_InvalidUnitPrice_When_Validated_Then_ShouldReturnInvalid(decimal invalidUnitPrice)
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            invalidUnitPrice,
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "UnitPrice" && e.ErrorCode == "GreaterThanValidator");
    }

    /// <summary>
    /// Tests that validation fails when UnitPrice exceeds maximum limit.
    /// </summary>
    [Theory(DisplayName = "Should fail validation when UnitPrice exceeds maximum limit")]
    [InlineData(10000.01)]
    [InlineData(20000)]
    [InlineData(50000)]
    public void Given_ExcessiveUnitPrice_When_Validated_Then_ShouldReturnInvalid(decimal excessiveUnitPrice)
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            excessiveUnitPrice,
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "UnitPrice" && e.ErrorCode == "LessThanOrEqualValidator");
    }

    /// <summary>
    /// Tests that validation passes for valid UnitPrice values.
    /// </summary>
    [Theory(DisplayName = "Should pass validation for valid UnitPrice values")]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(10000)]
    public void Given_ValidUnitPrice_When_Validated_Then_ShouldReturnValid(decimal validUnitPrice)
    {
        // Arrange
        var saleItem = new SaleItem(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            validUnitPrice,
            SaleItemTestData.GenerateValidDiscount()
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when Discount is negative.
    /// </summary>
    [Theory(DisplayName = "Should fail validation when Discount is negative")]
    [InlineData(-0.01)]
    [InlineData(-10)]
    [InlineData(-100)]
    public void Given_NegativeDiscount_When_Validated_Then_ShouldReturnInvalid(decimal negativeDiscount)
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            SaleItemTestData.GenerateValidUnitPrice(),
            negativeDiscount
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Discount" && e.ErrorCode == "GreaterThanOrEqualValidator");
    }

    /// <summary>
    /// Tests that validation fails when Discount exceeds 100%.
    /// </summary>
    [Theory(DisplayName = "Should fail validation when Discount exceeds 100%")]
    [InlineData(100.01)]
    [InlineData(150)]
    [InlineData(200)]
    public void Given_ExcessiveDiscount_When_Validated_Then_ShouldReturnInvalid(decimal excessiveDiscount)
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            SaleItemTestData.GenerateValidUnitPrice(),
            excessiveDiscount
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Discount" && e.ErrorCode == "LessThanOrEqualValidator");
    }

    /// <summary>
    /// Tests that validation passes for valid Discount values.
    /// </summary>
    [Theory(DisplayName = "Should pass validation for valid Discount values")]
    [InlineData(0)]
    [InlineData(25)]
    [InlineData(50)]
    [InlineData(100)]
    public void Given_ValidDiscount_When_Validated_Then_ShouldReturnValid(decimal validDiscount)
    {
        // Arrange
        var saleItem = new SaleItem(
            Guid.NewGuid(),
            SaleItemTestData.GenerateValidProductId(),
            SaleItemTestData.GenerateValidQuantity(),
            SaleItemTestData.GenerateValidUnitPrice(),
            validDiscount
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when multiple fields are invalid.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when multiple fields are invalid")]
    public void Given_MultipleInvalidFields_When_Validated_Then_ShouldReturnMultipleErrors()
    {
        // Arrange
        var saleItem = new SaleItem();
        saleItem.UpdateItemInfo(
            Guid.Empty, // Invalid SaleId
            Guid.Empty, // Invalid ProductId
            0, // Invalid Quantity
            0, // Invalid UnitPrice
            -10 // Invalid Discount
        );

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count > 1); // Multiple validation errors

        // Verify specific errors
        Assert.Contains(result.Errors, e => e.PropertyName == "SaleId");
        Assert.Contains(result.Errors, e => e.PropertyName == "ProductId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Quantity");
        Assert.Contains(result.Errors, e => e.PropertyName == "UnitPrice");
        Assert.Contains(result.Errors, e => e.PropertyName == "Discount");
    }

    /// <summary>
    /// Tests that validation passes for minimal valid SaleItem.
    /// </summary>
    [Fact(DisplayName = "Should pass validation for minimal valid SaleItem")]
    public void Given_MinimalValidSaleItem_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateMinimalSaleItem();

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes for maximal valid SaleItem.
    /// </summary>
    [Fact(DisplayName = "Should pass validation for maximal valid SaleItem")]
    public void Given_MaximalValidSaleItem_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateMaximalSaleItem();

        // Act
        var result = _validator.Validate(saleItem);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
} 