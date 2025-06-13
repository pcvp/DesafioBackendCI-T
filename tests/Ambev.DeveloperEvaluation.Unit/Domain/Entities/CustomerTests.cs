using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Customer entity class.
/// Tests cover business methods, status changes, and validation scenarios.
/// </summary>
public class CustomerTests
{
    /// <summary>
    /// Tests that when an inactive customer is activated, their status changes to Active.
    /// </summary>
    [Fact(DisplayName = "Customer status should change to Active when activated")]
    public void Given_InactiveCustomer_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Deactivate();
        Assert.False(customer.IsActive); // Ensure customer is inactive

        // Act
        customer.Activate();

        // Assert
        Assert.True(customer.IsActive);
        Assert.NotNull(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that when an active customer is deactivated, their status changes to Inactive.
    /// </summary>
    [Fact(DisplayName = "Customer status should change to Inactive when deactivated")]
    public void Given_ActiveCustomer_When_Deactivated_Then_StatusShouldBeInactive()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Activate();
        Assert.True(customer.IsActive); // Ensure customer is active

        // Act
        customer.Deactivate();

        // Assert
        Assert.False(customer.IsActive);
        Assert.NotNull(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdateContactInfo method updates all contact information correctly.
    /// </summary>
    [Fact(DisplayName = "UpdateContactInfo should update all contact information")]
    public void Given_ValidContactInfo_When_UpdateContactInfo_Then_ShouldUpdateAllFields()
    {
        // Arrange
        var customer = new Customer();
        var newName = CustomerTestData.GenerateValidName();
        var newEmail = CustomerTestData.GenerateValidEmail();
        var newPhone = CustomerTestData.GenerateValidPhone();

        // Act
        customer.UpdateContactInfo(newName, newEmail, newPhone);

        // Assert
        Assert.Equal(newName, customer.Name);
        Assert.Equal(newEmail, customer.Email);
        Assert.Equal(newPhone, customer.Phone);
        Assert.NotNull(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdateContactInfo method updates only name when email and phone are null.
    /// </summary>
    [Fact(DisplayName = "UpdateContactInfo should update only name when email and phone are null")]
    public void Given_OnlyName_When_UpdateContactInfo_Then_ShouldUpdateOnlyName()
    {
        // Arrange
        var customer = new Customer();
        var newName = CustomerTestData.GenerateValidName();

        // Act
        customer.UpdateContactInfo(newName);

        // Assert
        Assert.Equal(newName, customer.Name);
        Assert.Null(customer.Email);
        Assert.Null(customer.Phone);
        Assert.NotNull(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdateContactInfo method updates name and email when phone is null.
    /// </summary>
    [Fact(DisplayName = "UpdateContactInfo should update name and email when phone is null")]
    public void Given_NameAndEmail_When_UpdateContactInfo_Then_ShouldUpdateNameAndEmail()
    {
        // Arrange
        var customer = new Customer();
        var newName = CustomerTestData.GenerateValidName();
        var newEmail = CustomerTestData.GenerateValidEmail();

        // Act
        customer.UpdateContactInfo(newName, newEmail);

        // Assert
        Assert.Equal(newName, customer.Name);
        Assert.Equal(newEmail, customer.Email);
        Assert.Null(customer.Phone);
        Assert.NotNull(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that validation passes when all customer properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid customer data")]
    public void Given_ValidCustomerData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes for minimal customer data (only name).
    /// </summary>
    [Fact(DisplayName = "Validation should pass for minimal customer data")]
    public void Given_MinimalCustomerData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var customer = CustomerTestData.GenerateMinimalCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when customer name is empty.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when name is empty")]
    public void Given_EmptyName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo(""); // Empty name

        // Act
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Error.Equals("NotEmptyValidator"));
        Assert.Contains(result.Errors, e => e.Error.Equals("MinimumLengthValidator"));
    }

    /// <summary>
    /// Tests that validation fails when customer name is too short.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when name is too short")]
    public void Given_TooShortName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        customer.UpdateContactInfo("A"); // 1 character name

        // Act
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Error.Equals("MinimumLengthValidator"));
    }

    /// <summary>
    /// Tests that validation fails when customer name is too long.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when name is too long")]
    public void Given_TooLongName_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        var longName = CustomerTestData.GenerateLongName();
        customer.UpdateContactInfo(longName);

        // Act
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Error.Equals("MaximumLengthValidator"));
    }

    /// <summary>
    /// Tests that validation fails when email format is invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when email format is invalid")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var customer = new Customer();
        var invalidEmail = CustomerTestData.GenerateInvalidEmail();
        customer.UpdateContactInfo(CustomerTestData.GenerateValidName(), invalidEmail);

        // Act
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Error.Equals("PredicateValidator"));
    }

    /// <summary>
    /// Tests that validation fails when phone format is invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when phone format is invalid")]
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
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Error.Equals("RegularExpressionValidator"));
    }

    /// <summary>
    /// Tests that validation fails when multiple fields are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail when multiple fields are invalid")]
    public void Given_MultipleInvalidFields_When_Validated_Then_ShouldReturnMultipleErrors()
    {
        // Arrange
        var customer = CustomerTestData.GenerateInvalidCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.Count() > 1); // Multiple validation errors
    }

    /// <summary>
    /// Tests that customer is created with default values.
    /// </summary>
    [Fact(DisplayName = "Customer should be created with default values")]
    public void Given_NewCustomer_When_Created_Then_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        Assert.Equal(Guid.Empty, customer.Id);
        Assert.True(string.IsNullOrEmpty(customer.Name));
        Assert.Null(customer.Email);
        Assert.Null(customer.Phone);
        Assert.True(customer.IsActive); // Default should be active
        Assert.NotEqual(DateTime.MinValue, customer.CreatedAt); // Should be set in constructor
        Assert.Null(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that customer properties are set correctly during creation.
    /// </summary>
    [Fact(DisplayName = "Customer properties should be set correctly during creation")]
    public void Given_CustomerCreationData_When_Created_Then_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var name = CustomerTestData.GenerateValidName();
        var email = CustomerTestData.GenerateValidEmail();
        var phone = CustomerTestData.GenerateValidPhone();

        // Act
        var customer = new Customer();
        customer.UpdateContactInfo(name, email, phone);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(phone, customer.Phone);
        Assert.True(customer.IsActive);
        Assert.NotNull(customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdatedAt is set when customer is activated.
    /// </summary>
    [Fact(DisplayName = "UpdatedAt should be set when customer is activated")]
    public void Given_Customer_When_Activated_Then_UpdatedAtShouldBeSet()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Deactivate();
        var originalUpdatedAt = customer.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(1); // Ensure time difference
        customer.Activate();

        // Assert
        Assert.NotNull(customer.UpdatedAt);
        Assert.NotEqual(originalUpdatedAt, customer.UpdatedAt);
    }

    /// <summary>
    /// Tests that UpdatedAt is set when customer is deactivated.
    /// </summary>
    [Fact(DisplayName = "UpdatedAt should be set when customer is deactivated")]
    public void Given_Customer_When_Deactivated_Then_UpdatedAtShouldBeSet()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Activate();
        var originalUpdatedAt = customer.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(1); // Ensure time difference
        customer.Deactivate();

        // Assert
        Assert.NotNull(customer.UpdatedAt);
        Assert.NotEqual(originalUpdatedAt, customer.UpdatedAt);
    }


} 