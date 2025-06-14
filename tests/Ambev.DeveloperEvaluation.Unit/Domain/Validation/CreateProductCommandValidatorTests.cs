using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductCommandValidator"/> class.
/// </summary>
public class CreateProductCommandValidatorTests
{
    /// <summary>
    /// Tests that CreateProductCommandValidator accepts valid product data.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When validating CreateProductCommand Then validation succeeds")]
    public async Task CreateProductCommandValidator_ValidData_ShouldPassValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = "Valid Product Name",
            Price = 10.50m
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that CreateProductCommandValidator rejects empty name.
    /// </summary>
    [Fact(DisplayName = "Given empty name When validating CreateProductCommand Then validation fails")]
    public async Task CreateProductCommandValidator_EmptyName_ShouldFailValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = string.Empty,
            Price = 10.50m
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductCommand.Name));
    }

    /// <summary>
    /// Tests that CreateProductCommandValidator rejects null name.
    /// </summary>
    [Fact(DisplayName = "Given null name When validating CreateProductCommand Then validation fails")]
    public async Task CreateProductCommandValidator_NullName_ShouldFailValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = null!,
            Price = 10.50m
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductCommand.Name));
    }

    /// <summary>
    /// Tests that CreateProductCommandValidator rejects zero price.
    /// </summary>
    [Fact(DisplayName = "Given zero price When validating CreateProductCommand Then validation fails")]
    public async Task CreateProductCommandValidator_ZeroPrice_ShouldFailValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = "Valid Product Name",
            Price = 0
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateProductCommand.Price));
    }

    /// <summary>
    /// Tests that CreateProductCommandValidator rejects negative price.
    /// </summary>
    [Fact(DisplayName = "Given negative price When validating CreateProductCommand Then validation fails")]
    public async Task CreateProductCommandValidator_NegativePrice_ShouldFailValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = "Valid Product Name",
            Price = -10.50m
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateProductCommand.Price));
    }

    /// <summary>
    /// Tests that CreateProductCommandValidator accepts maximum valid price.
    /// </summary>
    [Fact(DisplayName = "Given maximum valid price When validating CreateProductCommand Then validation succeeds")]
    public async Task CreateProductCommandValidator_MaximumValidPrice_ShouldPassValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = "Valid Product Name",
            Price = 999999.99m
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that CreateProductCommandValidator accepts minimum valid price.
    /// </summary>
    [Fact(DisplayName = "Given minimum valid price When validating CreateProductCommand Then validation succeeds")]
    public async Task CreateProductCommandValidator_MinimumValidPrice_ShouldPassValidation()
    {
        // Given
        var validator = new CreateProductCommandValidator();
        var command = new CreateProductCommand
        {
            Name = "Valid Product Name",
            Price = 0.01m
        };

        // When
        var result = await validator.ValidateAsync(command);

        // Then
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
} 