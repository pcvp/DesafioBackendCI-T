using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the BranchValidator class.
/// Tests cover all validation rules and edge cases for Branch entity validation.
/// </summary>
public class BranchValidatorTests
{
    /// <summary>
    /// The validator instance used for testing.
    /// </summary>
    private readonly BranchValidator _validator = new();

    /// <summary>
    /// Tests that validation passes for a valid Branch entity.
    /// </summary>
    [Fact(DisplayName = "Given valid branch When validating Then validation should pass")]
    public void Given_ValidBranch_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation fails when Branch name is empty.
    /// </summary>
    [Fact(DisplayName = "Given branch with empty name When validating Then validation should fail")]
    public void Given_BranchWithEmptyName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithEmptyName();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Branch.Name));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("required"));
    }

    /// <summary>
    /// Tests that validation fails when Branch name is too short.
    /// </summary>
    [Fact(DisplayName = "Given branch with short name When validating Then validation should fail")]
    public void Given_BranchWithShortName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithShortName();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be(nameof(Branch.Name));
        result.Errors.First().ErrorMessage.Should().Contain("between 2 and 100 characters");
    }

    /// <summary>
    /// Tests that validation fails when Branch name is too long.
    /// </summary>
    [Fact(DisplayName = "Given branch with long name When validating Then validation should fail")]
    public void Given_BranchWithLongName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithLongName();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be(nameof(Branch.Name));
        result.Errors.First().ErrorMessage.Should().Contain("between 2 and 100 characters");
    }

    /// <summary>
    /// Tests that validation passes when Branch name has minimum valid length.
    /// </summary>
    [Fact(DisplayName = "Given branch with minimum valid name When validating Then validation should pass")]
    public void Given_BranchWithMinimumValidName_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithMinimumName();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation passes when Branch name has maximum valid length.
    /// </summary>
    [Fact(DisplayName = "Given branch with maximum valid name When validating Then validation should pass")]
    public void Given_BranchWithMaximumValidName_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithMaximumName();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation fails when Branch name is null.
    /// </summary>
    [Fact(DisplayName = "Given branch with null name When validating Then validation should fail")]
    public void Given_BranchWithNullName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = null!;

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Branch.Name));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("required"));
    }

    /// <summary>
    /// Tests that validation passes when Branch IsActive is true.
    /// </summary>
    [Fact(DisplayName = "Given branch with IsActive true When validating Then validation should pass")]
    public void Given_BranchWithIsActiveTrue_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.IsActive = true;

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation passes when Branch IsActive is false.
    /// </summary>
    [Fact(DisplayName = "Given branch with IsActive false When validating Then validation should pass")]
    public void Given_BranchWithIsActiveFalse_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.IsActive = false;

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation passes when Branch has valid Id.
    /// </summary>
    [Fact(DisplayName = "Given branch with valid Id When validating Then validation should pass")]
    public void Given_BranchWithValidId_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Id = Guid.NewGuid();

        // Act
        var result = _validator.Validate(branch);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
} 