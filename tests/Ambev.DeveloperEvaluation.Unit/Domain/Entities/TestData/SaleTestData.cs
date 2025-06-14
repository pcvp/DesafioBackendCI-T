using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale entity using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// </summary>
    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.ProductId, f => f.Random.Guid())
        .RuleFor(s => s.Quantity, f => f.Random.Int(1, 100))
        .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(1, 1000))
        .RuleFor(s => s.Discount, f => f.Random.Decimal(0, 50))
        .RuleFor(s => s.CreatedAt, f => f.Date.Recent())
        .RuleFor(s => s.UpdatedAt, f => f.Random.Bool() ? f.Date.Recent() : null);

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        return saleFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Sale entities with randomized data.
    /// </summary>
    /// <param name="count">Number of sales to generate</param>
    /// <returns>A list of valid Sale entities with randomly generated data.</returns>
    public static List<Sale> GenerateValidSales(int count = 5)
    {
        return saleFaker.Generate(count);
    }

    /// <summary>
    /// Generates a Sale with minimal valid data.
    /// </summary>
    /// <returns>A Sale with minimal valid data.</returns>
    public static Sale GenerateMinimalSale()
    {
        return new Sale
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            UnitPrice = 10.00m,
            Discount = 0m
        };
    }

    /// <summary>
    /// Generates a valid sale number.
    /// </summary>
    /// <returns>A valid sale number string.</returns>
    public static string GenerateValidSaleNumber()
    {
        return new Faker().Random.AlphaNumeric(10);
    }

    /// <summary>
    /// Generates an invalid sale number (empty).
    /// </summary>
    /// <returns>An empty string.</returns>
    public static string GenerateInvalidSaleNumber()
    {
        return string.Empty;
    }

    /// <summary>
    /// Generates a valid sale date.
    /// </summary>
    /// <returns>A valid DateTime for sale date.</returns>
    public static DateTime GenerateValidSaleDate()
    {
        return new Faker().Date.Recent();
    }

    /// <summary>
    /// Generates an invalid sale date (future date).
    /// </summary>
    /// <returns>A future DateTime.</returns>
    public static DateTime GenerateInvalidSaleDate()
    {
        return DateTime.Now.AddDays(1);
    }

    /// <summary>
    /// Generates a valid quantity.
    /// </summary>
    /// <returns>A valid quantity integer.</returns>
    public static int GenerateValidQuantity()
    {
        return new Faker().Random.Int(1, 100);
    }

    /// <summary>
    /// Generates an invalid quantity (zero or negative).
    /// </summary>
    /// <returns>An invalid quantity integer.</returns>
    public static int GenerateInvalidQuantity()
    {
        return new Faker().Random.Int(-10, 0);
    }

    /// <summary>
    /// Generates a valid unit price.
    /// </summary>
    /// <returns>A valid unit price decimal.</returns>
    public static decimal GenerateValidUnitPrice()
    {
        return new Faker().Random.Decimal(0.01m, 1000m);
    }

    /// <summary>
    /// Generates an invalid unit price (negative).
    /// </summary>
    /// <returns>An invalid unit price decimal.</returns>
    public static decimal GenerateInvalidUnitPrice()
    {
        return new Faker().Random.Decimal(-100m, -0.01m);
    }

    /// <summary>
    /// Generates a valid discount percentage.
    /// </summary>
    /// <returns>A valid discount percentage decimal.</returns>
    public static decimal GenerateValidDiscount()
    {
        return new Faker().Random.Decimal(0m, 100m);
    }

    /// <summary>
    /// Generates an invalid discount percentage (negative or over 100).
    /// </summary>
    /// <returns>An invalid discount percentage decimal.</returns>
    public static decimal GenerateInvalidDiscount()
    {
        var faker = new Faker();
        return faker.Random.Bool() 
            ? faker.Random.Decimal(-50m, -0.01m) 
            : faker.Random.Decimal(100.01m, 200m);
    }
} 