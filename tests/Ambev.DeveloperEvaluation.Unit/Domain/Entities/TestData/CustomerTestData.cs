using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Customer entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CustomerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Customer entities.
    /// The generated customers will have valid:
    /// - Name (using person names)
    /// - Email (valid format, optional)
    /// - Phone (international format, optional)
    /// - IsActive (true by default)
    /// - CreatedAt (current UTC time)
    /// </summary>
    private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
        .RuleFor(c => c.Name, f => f.Person.FullName)
        .RuleFor(c => c.Email, f => f.Random.Bool(0.8f) ? f.Internet.Email() : null) // 80% chance of having email
        .RuleFor(c => c.Phone, f => f.Random.Bool(0.7f) ? $"+{f.Random.Number(1, 9)}{f.Random.Number(1000000000, 2147483647)}" : null) // 70% chance of having phone
        .RuleFor(c => c.IsActive, f => true)
        .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow)
        .RuleFor(c => c.UpdatedAt, f => null);

    /// <summary>
    /// Generates a valid Customer entity with randomized data.
    /// The generated customer will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Customer entity with randomly generated data.</returns>
    public static Customer GenerateValidCustomer()
    {
        return CustomerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid customer name using Faker.
    /// The generated name will:
    /// - Be between 2 and 100 characters
    /// - Use realistic person names
    /// - Contain only valid characters
    /// </summary>
    /// <returns>A valid customer name.</returns>
    public static string GenerateValidName()
    {
        return new Faker().Person.FullName;
    }

    /// <summary>
    /// Generates a valid email address using Faker.
    /// The generated email will:
    /// - Follow the standard email format (user@domain.com)
    /// - Have valid characters in both local and domain parts
    /// - Have a valid TLD
    /// </summary>
    /// <returns>A valid email address.</returns>
    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }

    /// <summary>
    /// Generates a valid international phone number.
    /// The generated phone number will:
    /// - Start with country code (+1 to +9)
    /// - Have 10-15 digits total
    /// - Follow the format: +XXXXXXXXXXXX
    /// </summary>
    /// <returns>A valid international phone number.</returns>
    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+{faker.Random.Number(1, 9)}{faker.Random.Number(1000000000, 2147483647)}";
    }

    /// <summary>
    /// Generates an invalid customer name for testing negative scenarios.
    /// The generated name will:
    /// - Be empty or too short (less than 2 characters)
    /// This is useful for testing name validation error cases.
    /// </summary>
    /// <returns>An invalid customer name.</returns>
    public static string GenerateInvalidName()
    {
        return new Faker().Random.String2(0, 1); // 0 or 1 character
    }

    /// <summary>
    /// Generates a customer name that exceeds the maximum length limit.
    /// The generated name will:
    /// - Be longer than 100 characters
    /// - Contain random characters
    /// This is useful for testing name length validation error cases.
    /// </summary>
    /// <returns>A customer name that exceeds the maximum length limit.</returns>
    public static string GenerateLongName()
    {
        return new Faker().Random.String2(101, 150); // 101-150 characters
    }

    /// <summary>
    /// Generates an invalid email address for testing negative scenarios.
    /// The generated email will:
    /// - Not follow the standard email format
    /// - Not contain the @ symbol
    /// - Be a simple word or string
    /// This is useful for testing email validation error cases.
    /// </summary>
    /// <returns>An invalid email address.</returns>
    public static string GenerateInvalidEmail()
    {
        var faker = new Faker();
        return faker.Lorem.Word(); // Simple word without @ symbol
    }

    /// <summary>
    /// Generates an invalid phone number for testing negative scenarios.
    /// The generated phone number will:
    /// - Not follow the international phone number format
    /// - Not start with + or have incorrect length
    /// - Contain invalid characters
    /// This is useful for testing phone validation error cases.
    /// </summary>
    /// <returns>An invalid phone number.</returns>
    public static string GenerateInvalidPhone()
    {
        var faker = new Faker();
        return faker.Random.AlphaNumeric(5); // Short alphanumeric string
    }

    /// <summary>
    /// Generates a customer with all required fields for creation.
    /// The generated customer will have:
    /// - Valid name
    /// - Valid email
    /// - Valid phone
    /// - IsActive = true
    /// - CreatedAt = current UTC time
    /// </summary>
    /// <returns>A customer with all fields populated for creation scenarios.</returns>
    public static Customer GenerateCustomerForCreation()
    {
        var customer = new Customer();
        customer.UpdateContactInfo(
            GenerateValidName(),
            GenerateValidEmail(),
            GenerateValidPhone()
        );
        return customer;
    }

    /// <summary>
    /// Generates a customer with minimal required fields.
    /// The generated customer will have:
    /// - Valid name only
    /// - No email or phone
    /// - IsActive = true
    /// - CreatedAt = current UTC time
    /// </summary>
    /// <returns>A customer with only required fields populated.</returns>
    public static Customer GenerateMinimalCustomer()
    {
        var customer = new Customer();
        customer.UpdateContactInfo(GenerateValidName());
        return customer;
    }

    /// <summary>
    /// Generates a customer with invalid data for testing validation failures.
    /// The generated customer will have:
    /// - Invalid name (empty or too short)
    /// - Invalid email (if provided)
    /// - Invalid phone (if provided)
    /// </summary>
    /// <returns>A customer with invalid data for negative testing.</returns>
    public static Customer GenerateInvalidCustomer()
    {
        var customer = new Customer();
        customer.UpdateContactInfo(
            GenerateInvalidName(),
            GenerateInvalidEmail(),
            GenerateInvalidPhone()
        );
        return customer;
    }
} 