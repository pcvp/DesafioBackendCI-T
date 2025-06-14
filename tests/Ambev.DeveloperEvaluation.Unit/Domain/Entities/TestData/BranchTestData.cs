using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation for Branch entity to ensure consistency across tests.
/// </summary>
public static class BranchTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Branch entities.
    /// Uses Bogus library to create realistic test data with proper constraints.
    /// </summary>
    private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.Name, f => 
        {
            var companyName = f.Company.CompanyName();
            if (companyName.Length < 2)
                return f.Random.String2(10, 50);
            return companyName.Length > 50 ? companyName.Substring(0, 50) : companyName;
        })
        .RuleFor(b => b.IsActive, f => f.Random.Bool())
        .RuleFor(b => b.CreatedAt, f => f.Date.Past())
        .RuleFor(b => b.UpdatedAt, f => f.Date.Recent());

    /// <summary>
    /// Generates a single valid Branch entity with random data.
    /// </summary>
    /// <returns>A valid Branch entity</returns>
    public static Branch GenerateValidBranch()
    {
        return BranchFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Branch entities with random data.
    /// </summary>
    /// <param name="count">Number of branches to generate</param>
    /// <returns>List of valid Branch entities</returns>
    public static List<Branch> GenerateValidBranches(int count = 3)
    {
        return BranchFaker.Generate(count);
    }

    /// <summary>
    /// Generates a Branch entity with an empty name (invalid).
    /// </summary>
    /// <returns>A Branch entity with empty name</returns>
    public static Branch GenerateBranchWithEmptyName()
    {
        return BranchFaker.Clone()
            .RuleFor(b => b.Name, string.Empty)
            .Generate();
    }

    /// <summary>
    /// Generates a Branch entity with a name that is too short (invalid).
    /// </summary>
    /// <returns>A Branch entity with name too short</returns>
    public static Branch GenerateBranchWithShortName()
    {
        return BranchFaker.Clone()
            .RuleFor(b => b.Name, "A")
            .Generate();
    }

    /// <summary>
    /// Generates a Branch entity with a name that is too long (invalid).
    /// </summary>
    /// <returns>A Branch entity with name too long</returns>
    public static Branch GenerateBranchWithLongName()
    {
        return BranchFaker.Clone()
            .RuleFor(b => b.Name, f => new string('A', 101))
            .Generate();
    }

    /// <summary>
    /// Generates a Branch entity with minimum valid name length.
    /// </summary>
    /// <returns>A Branch entity with minimum valid name</returns>
    public static Branch GenerateBranchWithMinimumName()
    {
        return BranchFaker.Clone()
            .RuleFor(b => b.Name, "AB")
            .Generate();
    }

    /// <summary>
    /// Generates a Branch entity with maximum valid name length.
    /// </summary>
    /// <returns>A Branch entity with maximum valid name</returns>
    public static Branch GenerateBranchWithMaximumName()
    {
        return BranchFaker.Clone()
            .RuleFor(b => b.Name, new string('A', 100))
            .Generate();
    }
} 