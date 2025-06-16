using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItems;
using Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for SaleItem application layer using the Bogus library.
/// This class centralizes all test data generation for Commands, Queries, and Results
/// to ensure consistency across test cases.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Generates a valid CreateSaleItemCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateSaleItemCommand with randomly generated data.</returns>
    public static CreateSaleItemCommand GenerateValidCreateCommand()
    {
        var faker = new Faker();
        return new CreateSaleItemCommand
        {
            SaleId = faker.Random.Guid(),
            ProductId = faker.Random.Guid(),
            Quantity = faker.Random.Int(1, 20),
            UnitPrice = faker.Random.Decimal(1.01m, 1000m),
            Discount = faker.Random.Decimal(0, 50)
        };
    }

    /// <summary>
    /// Generates an invalid CreateSaleItemCommand for testing validation failures.
    /// </summary>
    /// <returns>A CreateSaleItemCommand with invalid data.</returns>
    public static CreateSaleItemCommand GenerateInvalidCreateCommand()
    {
        return new CreateSaleItemCommand
        {
            SaleId = Guid.Empty, // Invalid empty GUID
            ProductId = Guid.Empty, // Invalid empty GUID
            Quantity = 0, // Invalid quantity
            UnitPrice = 0, // Invalid zero price
            Discount = -10 // Invalid negative discount
        };
    }

    /// <summary>
    /// Generates a CreateSaleItemCommand with specific values.
    /// </summary>
    /// <param name="saleId">Sale ID</param>
    /// <param name="productId">Product ID</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="unitPrice">Unit price</param>
    /// <param name="discount">Discount percentage</param>
    /// <returns>A CreateSaleItemCommand with specified values.</returns>
    public static CreateSaleItemCommand GenerateCreateCommand(Guid saleId, Guid productId, int quantity, decimal unitPrice, decimal discount)
    {
        return new CreateSaleItemCommand
        {
            SaleId = saleId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount
        };
    }

    /// <summary>
    /// Generates a valid CreateSaleItemResult with randomized data.
    /// </summary>
    public static CreateSaleItemResult GenerateValidCreateSaleItemResult()
    {
        var faker = new Faker();
        var quantity = faker.Random.Int(1, 20);
        var unitPrice = faker.Random.Decimal(1, 1000);
        var discount = faker.Random.Decimal(0, 50);
        var totalAmount = quantity * unitPrice * (1 - discount / 100);

        return new CreateSaleItemResult
        {
            Id = faker.Random.Guid(),
            SaleId = faker.Random.Guid(),
            ProductId = faker.Random.Guid(),
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            TotalAmount = totalAmount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Generates a valid GetSaleItemQuery with randomized data.
    /// </summary>
    public static GetSaleItemQuery GenerateValidGetSaleItemQuery()
    {
        var faker = new Faker();
        return new GetSaleItemQuery
        {
            SaleId = faker.Random.Guid(),
            Id = faker.Random.Guid()
        };
    }

    /// <summary>
    /// Generates a GetSaleItemQuery with specific IDs.
    /// </summary>
    public static GetSaleItemQuery GenerateGetSaleItemQuery(Guid saleId, Guid itemId)
    {
        return new GetSaleItemQuery
        {
            SaleId = saleId,
            Id = itemId
        };
    }

    /// <summary>
    /// Generates a valid GetSaleItemResult with randomized data.
    /// </summary>
    public static GetSaleItemResult GenerateValidGetSaleItemResult()
    {
        var faker = new Faker();
        var quantity = faker.Random.Int(1, 20);
        var unitPrice = faker.Random.Decimal(1, 1000);
        var discount = faker.Random.Decimal(0, 50);
        var totalAmount = quantity * unitPrice * (1 - discount / 100);

        return new GetSaleItemResult
        {
            Id = faker.Random.Guid(),
            SaleId = faker.Random.Guid(),
            ProductId = faker.Random.Guid(),
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            TotalAmount = totalAmount,
            IsCancelled = false,
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Generates a valid GetSaleItemsQuery with randomized data.
    /// </summary>
    public static GetSaleItemsQuery GenerateValidGetSaleItemsQuery()
    {
        var faker = new Faker();
        return new GetSaleItemsQuery
        {
            SaleId = faker.Random.Guid(),
            Page = faker.Random.Int(1, 5),
            Size = faker.Random.Int(5, 20)
        };
    }

    /// <summary>
    /// Generates a GetSaleItemsQuery with specific parameters.
    /// </summary>
    public static GetSaleItemsQuery GenerateGetSaleItemsQuery(Guid saleId, int page = 1, int size = 10)
    {
        return new GetSaleItemsQuery
        {
            SaleId = saleId,
            Page = page,
            Size = size
        };
    }

    /// <summary>
    /// Generates a valid GetSaleItemsResult with randomized data.
    /// </summary>
    public static GetSaleItemsResult GenerateValidGetSaleItemsResult(int itemCount = 2)
    {
        var faker = new Faker();
        var items = new List<SaleItemResultDto>();

        for (int i = 0; i < itemCount; i++)
        {
            var quantity = faker.Random.Int(1, 20);
            var unitPrice = faker.Random.Decimal(1, 1000);
            var discount = faker.Random.Decimal(0, 50);
            var totalAmount = quantity * unitPrice * (1 - discount / 100);

            items.Add(new SaleItemResultDto
            {
                Id = faker.Random.Guid(),
                SaleId = faker.Random.Guid(),
                ProductId = faker.Random.Guid(),
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                TotalAmount = totalAmount,
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddHours(-i),
                UpdatedAt = null
            });
        }

        return new GetSaleItemsResult
        {
            Items = items,
            CurrentPage = 1,
            TotalPages = 1,
            TotalCount = itemCount,
            HasNext = false,
            HasPrevious = false
        };
    }

    /// <summary>
    /// Generates a valid UpdateSaleItemCommand with randomized data.
    /// </summary>
    public static UpdateSaleItemCommand GenerateValidUpdateSaleItemCommand()
    {
        var faker = new Faker();
        return new UpdateSaleItemCommand
        {
            Id = faker.Random.Guid(),
            ProductId = faker.Random.Guid(),
            Quantity = faker.Random.Int(1, 20),
            Discount = faker.Random.Decimal(0, 50)
        };
    }

    /// <summary>
    /// Generates an UpdateSaleItemCommand with specific IDs.
    /// </summary>
    public static UpdateSaleItemCommand GenerateUpdateSaleItemCommand(Guid itemId)
    {
        var faker = new Faker();
        return new UpdateSaleItemCommand
        {
            Id = itemId,
            ProductId = faker.Random.Guid(),
            Quantity = faker.Random.Int(1, 20),
            Discount = faker.Random.Decimal(0, 50)
        };
    }

    /// <summary>
    /// Generates an invalid UpdateSaleItemCommand for testing validation failures.
    /// </summary>
    public static UpdateSaleItemCommand GenerateInvalidUpdateSaleItemCommand()
    {
        return new UpdateSaleItemCommand
        {
            Id = Guid.Empty, // Invalid empty GUID
            ProductId = Guid.Empty, // Invalid empty GUID
            Quantity = 0, // Invalid quantity
            Discount = -10 // Invalid negative discount
        };
    }

    /// <summary>
    /// Generates a valid UpdateSaleItemResult with randomized data.
    /// </summary>
    public static UpdateSaleItemResult GenerateValidUpdateSaleItemResult()
    {
        var faker = new Faker();
        var quantity = faker.Random.Int(1, 20);
        var unitPrice = faker.Random.Decimal(1, 1000);
        var discount = faker.Random.Decimal(0, 50);
        var totalAmount = quantity * unitPrice * (1 - discount / 100);

        return new UpdateSaleItemResult
        {
            Id = faker.Random.Guid(),
            SaleId = faker.Random.Guid(),
            ProductId = faker.Random.Guid(),
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount,
            TotalAmount = totalAmount,
            IsCancelled = false,
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Generates a valid DeleteSaleItemCommand with randomized data.
    /// </summary>
    public static DeleteSaleItemCommand GenerateValidDeleteSaleItemCommand()
    {
        var faker = new Faker();
        return new DeleteSaleItemCommand
        {
            Id = faker.Random.Guid(),
            SaleId = faker.Random.Guid()
        };
    }

    /// <summary>
    /// Generates a DeleteSaleItemCommand with specific IDs.
    /// </summary>
    public static DeleteSaleItemCommand GenerateDeleteSaleItemCommand(Guid saleId, Guid itemId)
    {
        return new DeleteSaleItemCommand
        {
            Id = itemId,
            SaleId = saleId
        };
    }

    /// <summary>
    /// Generates an invalid DeleteSaleItemCommand for testing validation failures.
    /// </summary>
    public static DeleteSaleItemCommand GenerateInvalidDeleteSaleItemCommand()
    {
        return new DeleteSaleItemCommand
        {
            Id = Guid.Empty, // Invalid
            SaleId = Guid.Empty // Invalid
        };
    }
} 