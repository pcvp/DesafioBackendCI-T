using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for Sale-related operations using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand entities.
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.ProductId, f => f.Random.Guid())
        .RuleFor(s => s.Quantity, f => f.Random.Int(1, 100))
        .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(1, 1000))
        .RuleFor(s => s.Discount, f => f.Random.Decimal(0, 50))
        .RuleFor(s => s.TotalAmount, (f, s) => s.Quantity * s.UnitPrice * (1 - s.Discount / 100))
        .RuleFor(s => s.TotalSaleAmount, (f, s) => s.TotalAmount);

    /// <summary>
    /// Configures the Faker to generate valid GetSaleQuery entities.
    /// </summary>
    private static readonly Faker<GetSaleQuery> getSaleQueryFaker = new Faker<GetSaleQuery>()
        .RuleFor(s => s.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid GetSalesQuery entities.
    /// </summary>
    private static readonly Faker<GetSalesQuery> getSalesQueryFaker = new Faker<GetSalesQuery>()
        .RuleFor(s => s.Page, f => f.Random.Int(1, 10))
        .RuleFor(s => s.Size, f => f.Random.Int(1, 100))
        .RuleFor(s => s.CustomerId, f => f.Random.Bool() ? f.Random.Guid() : null)
        .RuleFor(s => s.BranchId, f => f.Random.Bool() ? f.Random.Guid() : null)
        .RuleFor(s => s.StartDate, f => f.Random.Bool() ? f.Date.Past() : null)
        .RuleFor(s => s.EndDate, f => f.Random.Bool() ? f.Date.Recent() : null)
        .RuleFor(s => s.IsCancelled, f => f.Random.Bool() ? f.Random.Bool() : null);

    /// <summary>
    /// Configures the Faker to generate valid UpdateSaleCommand entities.
    /// </summary>
    private static readonly Faker<UpdateSaleCommand> updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.ProductId, f => f.Random.Guid())
        .RuleFor(s => s.Quantity, f => f.Random.Int(1, 100))
        .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(1, 1000))
        .RuleFor(s => s.Discount, f => f.Random.Decimal(0, 50))
        .RuleFor(s => s.TotalAmount, (f, s) => s.Quantity * s.UnitPrice * (1 - s.Discount / 100))
        .RuleFor(s => s.TotalSaleAmount, (f, s) => s.TotalAmount)
        .RuleFor(s => s.IsCancelled, f => f.Random.Bool());

    /// <summary>
    /// Configures the Faker to generate valid DeleteSaleCommand entities.
    /// </summary>
    private static readonly Faker<DeleteSaleCommand> deleteSaleCommandFaker = new Faker<DeleteSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// </summary>
    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale(
            f.Random.AlphaNumeric(10),
            f.Date.Recent(),
            f.Random.Guid(),
            f.Random.Guid(),
            f.Random.Guid(),
            f.Random.Int(1, 100),
            f.Random.Decimal(1, 1000),
            f.Random.Decimal(0, 50)
        ))
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.CreatedAt, f => f.Date.Recent())
        .RuleFor(s => s.UpdatedAt, f => f.Random.Bool() ? f.Date.Recent() : null);

    /// <summary>
    /// Generates a valid CreateSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateSaleCommand with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCreateCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetSaleQuery with randomized data.
    /// </summary>
    /// <returns>A valid GetSaleQuery with randomly generated data.</returns>
    public static GetSaleQuery GenerateValidGetQuery()
    {
        return getSaleQueryFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetSalesQuery with randomized data.
    /// </summary>
    /// <returns>A valid GetSalesQuery with randomly generated data.</returns>
    public static GetSalesQuery GenerateValidGetSalesQuery()
    {
        return getSalesQueryFaker.Generate();
    }

    /// <summary>
    /// Generates a valid UpdateSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid UpdateSaleCommand with randomly generated data.</returns>
    public static UpdateSaleCommand GenerateValidUpdateCommand()
    {
        return updateSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid DeleteSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid DeleteSaleCommand with randomly generated data.</returns>
    public static DeleteSaleCommand GenerateValidDeleteCommand()
    {
        return deleteSaleCommandFaker.Generate();
    }

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
    /// Generates a CreateSaleCommand with invalid data for testing validation.
    /// </summary>
    /// <returns>A CreateSaleCommand with invalid data.</returns>
    public static CreateSaleCommand GenerateInvalidCreateCommand()
    {
        return new CreateSaleCommand
        {
            SaleNumber = "", // Invalid: empty
            SaleDate = DateTime.MinValue, // Invalid: min value
            CustomerId = Guid.Empty, // Invalid: empty guid
            BranchId = Guid.Empty, // Invalid: empty guid
            ProductId = Guid.Empty, // Invalid: empty guid
            Quantity = 0, // Invalid: zero quantity
            UnitPrice = -1, // Invalid: negative price
            Discount = -5, // Invalid: negative discount
            TotalAmount = 0,
            TotalSaleAmount = 0
        };
    }

    /// <summary>
    /// Generates an UpdateSaleCommand with invalid data for testing validation.
    /// </summary>
    /// <returns>An UpdateSaleCommand with invalid data.</returns>
    public static UpdateSaleCommand GenerateInvalidUpdateCommand()
    {
        return new UpdateSaleCommand
        {
            Id = Guid.Empty, // Invalid: empty guid
            SaleNumber = "", // Invalid: empty
            SaleDate = DateTime.MinValue, // Invalid: min value
            CustomerId = Guid.Empty, // Invalid: empty guid
            BranchId = Guid.Empty, // Invalid: empty guid
            ProductId = Guid.Empty, // Invalid: empty guid
            Quantity = 0, // Invalid: zero quantity
            UnitPrice = -1, // Invalid: negative price
            Discount = -5, // Invalid: negative discount
            TotalAmount = 0,
            TotalSaleAmount = 0,
            IsCancelled = false
        };
    }
} 