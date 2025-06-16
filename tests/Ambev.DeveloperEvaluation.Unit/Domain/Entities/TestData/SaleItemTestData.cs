using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for SaleItem entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Configures the Faker to generate valid SaleItem entities.
    /// </summary>
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .CustomInstantiator(f => new SaleItem(
            f.Random.Guid(),
            f.Random.Guid(), // ProductId instead of ProductName
            f.Random.Int(1, 20),
            f.Random.Decimal(1, 1000),
            f.Random.Decimal(0, 50)
        ));

    /// <summary>
    /// Generates a valid SaleItem entity with randomized data.
    /// </summary>
    public static SaleItem GenerateValidSaleItem()
    {
        return SaleItemFaker.Generate();
    }

    /// <summary>
    /// Generates a valid SaleItem with specific SaleId.
    /// </summary>
    public static SaleItem GenerateValidSaleItemWithSaleId(Guid saleId)
    {
        var faker = new Faker();
        return new SaleItem(
            saleId,
            faker.Random.Guid(), // ProductId instead of ProductName
            faker.Random.Int(1, 20),
            faker.Random.Decimal(1, 1000),
            faker.Random.Decimal(0, 50)
        );
    }

    /// <summary>
    /// Generates a valid product ID using Faker.
    /// </summary>
    public static Guid GenerateValidProductId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates an invalid product ID for testing negative scenarios.
    /// </summary>
    public static Guid GenerateInvalidProductId()
    {
        return Guid.Empty; // Invalid empty GUID
    }

    /// <summary>
    /// Generates a valid quantity.
    /// </summary>
    public static int GenerateValidQuantity()
    {
        return new Faker().Random.Int(1, 20);
    }

    /// <summary>
    /// Generates an invalid quantity (zero or negative).
    /// </summary>
    public static int GenerateInvalidQuantity()
    {
        return new Faker().Random.Int(-10, 0);
    }

    /// <summary>
    /// Generates a quantity that exceeds the maximum limit.
    /// </summary>
    public static int GenerateExcessiveQuantity()
    {
        return new Faker().Random.Int(21, 50);
    }

    /// <summary>
    /// Generates a valid unit price.
    /// </summary>
    public static decimal GenerateValidUnitPrice()
    {
        return new Faker().Random.Decimal(0.01m, 10000m);
    }

    /// <summary>
    /// Generates an invalid unit price (zero or negative).
    /// </summary>
    public static decimal GenerateInvalidUnitPrice()
    {
        return new Faker().Random.Decimal(-100m, 0m);
    }

    /// <summary>
    /// Generates a unit price that exceeds the maximum limit.
    /// </summary>
    public static decimal GenerateExcessiveUnitPrice()
    {
        return new Faker().Random.Decimal(10001m, 20000m);
    }

    /// <summary>
    /// Generates a valid discount percentage.
    /// </summary>
    public static decimal GenerateValidDiscount()
    {
        return new Faker().Random.Decimal(0m, 100m);
    }

    /// <summary>
    /// Generates an invalid discount (negative).
    /// </summary>
    public static decimal GenerateInvalidDiscount()
    {
        return new Faker().Random.Decimal(-50m, -0.01m);
    }

    /// <summary>
    /// Generates a discount that exceeds 100%.
    /// </summary>
    public static decimal GenerateExcessiveDiscount()
    {
        return new Faker().Random.Decimal(100.01m, 200m);
    }

    /// <summary>
    /// Generates a SaleItem with minimal required fields.
    /// </summary>
    public static SaleItem GenerateMinimalSaleItem()
    {
        var faker = new Faker();
        return new SaleItem(
            faker.Random.Guid(),
            faker.Random.Guid(), // ProductId
            1,    // Minimum quantity
            0.01m, // Minimum unit price
            0m    // No discount
        );
    }

    /// <summary>
    /// Generates a SaleItem with maximum allowed values.
    /// </summary>
    public static SaleItem GenerateMaximalSaleItem()
    {
        var faker = new Faker();
        return new SaleItem(
            faker.Random.Guid(),
            faker.Random.Guid(), // ProductId
            20,      // Maximum quantity
            10000m,  // Maximum unit price
            100m     // Maximum discount
        );
    }

    /// <summary>
    /// Generates a SaleItem with invalid data for testing validation failures.
    /// </summary>
    public static SaleItem GenerateInvalidSaleItem()
    {
        var faker = new Faker();
        return new SaleItem(
            Guid.Empty, // Invalid SaleId
            GenerateInvalidProductId(),
            GenerateInvalidQuantity(),
            GenerateInvalidUnitPrice(),
            GenerateInvalidDiscount()
        );
    }

    /// <summary>
    /// Generates a cancelled SaleItem.
    /// </summary>
    public static SaleItem GenerateCancelledSaleItem()
    {
        var saleItem = GenerateValidSaleItem();
        saleItem.Cancel();
        return saleItem;
    }
} 