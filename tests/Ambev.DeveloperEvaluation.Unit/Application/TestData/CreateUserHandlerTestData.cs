using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateUserHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateUserCommand entities.
    /// The generated commands will have valid:
    /// - Username (using internet usernames)
    /// - Password (meeting complexity requirements)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Status (Active or Suspended)
    /// - Role (Customer or Admin)
    /// </summary>
    private static readonly Faker<CreateUserCommand> createUserHandlerFaker = new Faker<CreateUserCommand>()
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin));

    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// </summary>
    private static readonly Faker<User> userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin));

    /// <summary>
    /// Configures the Faker to generate valid CreateUserResult entities.
    /// </summary>
    private static readonly Faker<CreateUserResult> createUserResultFaker = new Faker<CreateUserResult>()
        .RuleFor(r => r.Id, f => f.Random.Guid());

    /// <summary>
    /// Generates a valid CreateUserCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateUserCommand with randomly generated data.</returns>
    public static CreateUserCommand GenerateValidCommand()
    {
        return createUserHandlerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static User GenerateValidUser()
    {
        return userFaker.Generate();
    }

    /// <summary>
    /// Generates a valid CreateUserResult with randomized data.
    /// </summary>
    /// <returns>A valid CreateUserResult with randomly generated data.</returns>
    public static CreateUserResult GenerateValidCreateUserResult()
    {
        return createUserResultFaker.Generate();
    }
}
