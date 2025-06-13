using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the CustomerValidator class.
/// Tests cover validation scenarios for customer properties including name, email, and phone.
/// </summary>
public class CustomerValidatorTests
{
    private readonly CustomerValidator _validator;

    public CustomerValidatorTests()
    {
        _validator = new CustomerValidator();
    }

    /// <summary>
    /// Tests that validation passes when all customer properties are valid.
    /// </summary>
    [Fact(DisplayName = "Should pass validation when all customer properties are valid")]
    public void Given_ValidCustomer_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes for minimal customer data (only name).
    /// </summary>
    [Fact(DisplayName = "Should pass validation for minimal customer data")]
    public void Given_MinimalCustomer_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateMinimalCustomer();

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when customer name is null.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when name is null")]
    public void Given_NullName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        // Name is null by default

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorCode == "NotEmptyValidator");
    }

    /// <summary>
    /// Tests that validation fails when customer name is empty.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when name is empty")]
    public void Given_EmptyName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo("");

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorCode == "NotEmptyValidator");
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorCode == "MinimumLengthValidator");
    }

    /// <summary>
    /// Tests that validation fails when customer name is too short.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when name is too short")]
    public void Given_TooShortName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo("A"); // 1 character

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorCode == "MinimumLengthValidator");
    }

    /// <summary>
    /// Tests that validation fails when customer name is too long.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when name is too long")]
    public void Given_TooLongName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        var longName = CustomerTestData.GenerateLongName();
        customer.UpdateContactInfo(longName);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorCode == "MaximumLengthValidator");
    }

    /// <summary>
    /// Tests that validation passes when name is at minimum length (2 characters).
    /// </summary>
    [Fact(DisplayName = "Should pass validation when name is at minimum length")]
    public void Given_MinimumLengthName_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo("AB"); // 2 characters

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes when name is at maximum length (100 characters).
    /// </summary>
    [Fact(DisplayName = "Should pass validation when name is at maximum length")]
    public void Given_MaximumLengthName_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = new Customer();
        var maxName = new string('A', 100); // 100 characters
        customer.UpdateContactInfo(maxName);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes when email is null (optional field).
    /// </summary>
    [Fact(DisplayName = "Should pass validation when email is null")]
    public void Given_NullEmail_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName());

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes when email is empty (optional field).
    /// </summary>
    [Fact(DisplayName = "Should pass validation when email is empty")]
    public void Given_EmptyEmail_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), "");

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when email format is invalid.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when email format is invalid")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        var invalidEmail = CustomerTestData.GenerateInvalidEmail();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), invalidEmail);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorCode == "PredicateValidator");
    }

    /// <summary>
    /// Tests that validation passes for various valid email formats.
    /// </summary>
    [Theory(DisplayName = "Should pass validation for valid email formats")]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("test+tag@example.org")]
    [InlineData("123@numbers.com")]
    [InlineData("a@b.co")]
    public void Given_ValidEmailFormats_When_Validated_Then_ShouldReturnValid(string email)
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), email);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails for various invalid email formats.
    /// </summary>
    [Theory(DisplayName = "Should fail validation for invalid email formats")]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user@domain")]
    [InlineData("user.domain.com")]
    [InlineData("user@@domain.com")]
    public void Given_InvalidEmailFormats_When_Validated_Then_ShouldReturnInvalid(string email)
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), email);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorCode == "PredicateValidator");
    }

    /// <summary>
    /// Tests that validation passes when phone is null (optional field).
    /// </summary>
    [Fact(DisplayName = "Should pass validation when phone is null")]
    public void Given_NullPhone_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName());

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes when phone is empty (optional field).
    /// </summary>
    [Fact(DisplayName = "Should pass validation when phone is empty")]
    public void Given_EmptyPhone_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), null, "");

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when phone format is invalid.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when phone format is invalid")]
    public void Given_InvalidPhoneFormat_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        var invalidPhone = CustomerTestData.GenerateInvalidPhone();
        customer.UpdateContactInfo(
            CustomerTestData.GenerateValidName(), 
            CustomerTestData.GenerateValidEmail(), 
            invalidPhone);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Phone" && e.ErrorCode == "RegularExpressionValidator");
    }

    /// <summary>
    /// Tests that validation passes for various valid international phone formats.
    /// </summary>
    [Theory(DisplayName = "Should pass validation for valid international phone formats")]
    [InlineData("+12345678901")]
    [InlineData("+123456789012")]
    [InlineData("+1234567890123")]
    [InlineData("+12345678901234")]
    [InlineData("+123456789012345")]
    [InlineData("+5511987654321")]
    [InlineData("+4412345678901")]
    public void Given_ValidPhoneFormats_When_Validated_Then_ShouldReturnValid(string phone)
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), null, phone);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails for various invalid phone formats.
    /// </summary>
    [Theory(DisplayName = "Should fail validation for invalid phone formats")]
    [InlineData("123456789")] // No + prefix
    [InlineData("+123")] // Too short
    [InlineData("+1234567890")] // Too short (only 10 digits)
    [InlineData("+1234567890123456")] // Too long
    [InlineData("1234567890")] // No + prefix
    [InlineData("+abc1234567890")] // Contains letters
    [InlineData("+(11)98765-4321")] // Contains special characters
    [InlineData("+55 11 98765-4321")] // Contains spaces and dashes
    public void Given_InvalidPhoneFormats_When_Validated_Then_ShouldReturnInvalid(string phone)
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), null, phone);

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "Phone" && e.ErrorCode == "RegularExpressionValidator");
    }

    /// <summary>
    /// Tests that validation fails when multiple fields are invalid.
    /// </summary>
    [Fact(DisplayName = "Should fail validation when multiple fields are invalid")]
    public void Given_MultipleInvalidFields_When_Validated_Then_ShouldReturnMultipleErrors()
    {
        // Arrange
        var customer = CustomerTestData.GenerateInvalidCustomer();

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count > 1); // Multiple validation errors
    }

    /// <summary>
    /// Tests that validation error messages are descriptive and helpful.
    /// </summary>
    [Fact(DisplayName = "Should provide descriptive error messages")]
    public void Given_InvalidCustomer_When_Validated_Then_ShouldProvideDescriptiveErrors()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo("", "invalid-email", "invalid-phone");

        // Act
        var result = _validator.Validate(customer);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        
        // Check that error messages are not empty
        foreach (var error in result.Errors)
        {
            Assert.NotNull(error.ErrorMessage);
            Assert.NotEmpty(error.ErrorMessage);
        }
    }
} 