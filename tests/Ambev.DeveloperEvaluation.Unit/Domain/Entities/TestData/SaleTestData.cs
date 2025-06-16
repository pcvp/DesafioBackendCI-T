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
        .CustomInstantiator(f => new Sale(
            f.Random.AlphaNumeric(10),
            f.Date.Recent(),
            f.Random.Guid(),
            f.Random.Guid()
        ))
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.CreatedAt, f => f.Date.Recent())
        .RuleFor(s => s.UpdatedAt, f => f.Random.Bool() ? f.Date.Recent() : null)
        .FinishWith((f, s) => {
            // Add some sale items
            var itemCount = f.Random.Int(1, 3);
            for (int i = 0; i < itemCount; i++)
            {
                var saleItem = new SaleItem(
                    s.Id,
                    f.Random.Guid(),
                    f.Random.Int(1, 20),
                    f.Random.Decimal(1, 1000),
                    f.Random.Decimal(0, 50)
                );
                s.AddItem(saleItem);
            }
        });

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
    /// Generates a Sale with minimal valid data (no items).
    /// </summary>
    /// <returns>A Sale with minimal valid data.</returns>
    public static Sale GenerateMinimalSale()
    {
        return new Sale(
            "SALE001",
            DateTime.Now,
            Guid.NewGuid(),
            Guid.NewGuid()
        );
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