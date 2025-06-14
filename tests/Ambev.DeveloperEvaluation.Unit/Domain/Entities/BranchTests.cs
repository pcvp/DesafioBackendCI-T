using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Branch entity class.
/// Tests cover entity creation, property validation, and business rules.
/// </summary>
public class BranchTests
{
    /// <summary>
    /// Tests that a valid Branch can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid branch data When creating branch Then branch should be created successfully")]
    public void Given_ValidBranchData_When_CreatingBranch_Then_BranchShouldBeCreatedSuccessfully()
    {
        // Arrange
        var branchData = BranchTestData.GenerateValidBranch();

        // Act
        var branch = new Branch
        {
            Id = branchData.Id,
            Name = branchData.Name,
            IsActive = branchData.IsActive,
            CreatedAt = branchData.CreatedAt,
            UpdatedAt = branchData.UpdatedAt
        };

        // Assert
        branch.Should().NotBeNull();
        branch.Id.Should().Be(branchData.Id);
        branch.Name.Should().Be(branchData.Name);
        branch.IsActive.Should().Be(branchData.IsActive);
        branch.CreatedAt.Should().Be(branchData.CreatedAt);
        branch.UpdatedAt.Should().Be(branchData.UpdatedAt);
    }

    /// <summary>
    /// Tests that Branch properties can be set and retrieved correctly.
    /// </summary>
    [Fact(DisplayName = "Given branch instance When setting properties Then properties should be set correctly")]
    public void Given_BranchInstance_When_SettingProperties_Then_PropertiesShouldBeSetCorrectly()
    {
        // Arrange
        var branch = new Branch();
        var id = Guid.NewGuid();
        var name = "Test Branch";
        var isActive = true;
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddMinutes(5);

        // Act
        branch.Id = id;
        branch.Name = name;
        branch.IsActive = isActive;
        branch.CreatedAt = createdAt;
        branch.UpdatedAt = updatedAt;

        // Assert
        branch.Id.Should().Be(id);
        branch.Name.Should().Be(name);
        branch.IsActive.Should().Be(isActive);
        branch.CreatedAt.Should().Be(createdAt);
        branch.UpdatedAt.Should().Be(updatedAt);
    }

    /// <summary>
    /// Tests that Branch can be created with minimum valid name length.
    /// </summary>
    [Fact(DisplayName = "Given minimum valid name When creating branch Then branch should be created successfully")]
    public void Given_MinimumValidName_When_CreatingBranch_Then_BranchShouldBeCreatedSuccessfully()
    {
        // Arrange
        var branchData = BranchTestData.GenerateBranchWithMinimumName();

        // Act
        var branch = new Branch
        {
            Id = branchData.Id,
            Name = branchData.Name,
            IsActive = branchData.IsActive,
            CreatedAt = branchData.CreatedAt,
            UpdatedAt = branchData.UpdatedAt
        };

        // Assert
        branch.Should().NotBeNull();
        branch.Name.Should().Be(branchData.Name);
        branch.Name.Length.Should().Be(2);
    }

    /// <summary>
    /// Tests that Branch can be created with maximum valid name length.
    /// </summary>
    [Fact(DisplayName = "Given maximum valid name When creating branch Then branch should be created successfully")]
    public void Given_MaximumValidName_When_CreatingBranch_Then_BranchShouldBeCreatedSuccessfully()
    {
        // Arrange
        var branchData = BranchTestData.GenerateBranchWithMaximumName();

        // Act
        var branch = new Branch
        {
            Id = branchData.Id,
            Name = branchData.Name,
            IsActive = branchData.IsActive,
            CreatedAt = branchData.CreatedAt,
            UpdatedAt = branchData.UpdatedAt
        };

        // Assert
        branch.Should().NotBeNull();
        branch.Name.Should().Be(branchData.Name);
        branch.Name.Length.Should().Be(100);
    }

    /// <summary>
    /// Tests that Branch can be created with IsActive set to true.
    /// </summary>
    [Fact(DisplayName = "Given IsActive true When creating branch Then branch should be active")]
    public void Given_IsActiveTrue_When_CreatingBranch_Then_BranchShouldBeActive()
    {
        // Arrange & Act
        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            Name = "Active Branch",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        branch.IsActive.Should().BeTrue();
    }

    /// <summary>
    /// Tests that Branch can be created with IsActive set to false.
    /// </summary>
    [Fact(DisplayName = "Given IsActive false When creating branch Then branch should be inactive")]
    public void Given_IsActiveFalse_When_CreatingBranch_Then_BranchShouldBeInactive()
    {
        // Arrange & Act
        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            Name = "Inactive Branch",
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        branch.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Tests that Branch Id can be set to a valid Guid.
    /// </summary>
    [Fact(DisplayName = "Given valid Guid When setting Id Then Id should be set correctly")]
    public void Given_ValidGuid_When_SettingId_Then_IdShouldBeSetCorrectly()
    {
        // Arrange
        var branch = new Branch();
        var expectedId = Guid.NewGuid();

        // Act
        branch.Id = expectedId;

        // Assert
        branch.Id.Should().Be(expectedId);
        branch.Id.Should().NotBe(Guid.Empty);
    }

    /// <summary>
    /// Tests that Branch CreatedAt can be set to a valid DateTime.
    /// </summary>
    [Fact(DisplayName = "Given valid DateTime When setting CreatedAt Then CreatedAt should be set correctly")]
    public void Given_ValidDateTime_When_SettingCreatedAt_Then_CreatedAtShouldBeSetCorrectly()
    {
        // Arrange
        var branch = new Branch();
        var expectedCreatedAt = DateTime.UtcNow;

        // Act
        branch.CreatedAt = expectedCreatedAt;

        // Assert
        branch.CreatedAt.Should().Be(expectedCreatedAt);
    }

    /// <summary>
    /// Tests that Branch UpdatedAt can be set to a valid DateTime.
    /// </summary>
    [Fact(DisplayName = "Given valid DateTime When setting UpdatedAt Then UpdatedAt should be set correctly")]
    public void Given_ValidDateTime_When_SettingUpdatedAt_Then_UpdatedAtShouldBeSetCorrectly()
    {
        // Arrange
        var branch = new Branch();
        var expectedUpdatedAt = DateTime.UtcNow;

        // Act
        branch.UpdatedAt = expectedUpdatedAt;

        // Assert
        branch.UpdatedAt.Should().Be(expectedUpdatedAt);
    }

    /// <summary>
    /// Tests that multiple Branch instances can be created with different data.
    /// </summary>
    [Fact(DisplayName = "Given multiple branch data When creating branches Then all branches should be created successfully")]
    public void Given_MultipleBranchData_When_CreatingBranches_Then_AllBranchesShouldBeCreatedSuccessfully()
    {
        // Arrange
        var branchesData = BranchTestData.GenerateValidBranches(3);

        // Act
        var branches = branchesData.Select(data => new Branch
        {
            Id = data.Id,
            Name = data.Name,
            IsActive = data.IsActive,
            CreatedAt = data.CreatedAt,
            UpdatedAt = data.UpdatedAt
        }).ToList();

        // Assert
        branches.Should().HaveCount(3);
        branches.Should().OnlyContain(b => !string.IsNullOrEmpty(b.Name));
        branches.Should().OnlyContain(b => b.Id != Guid.Empty);
        branches.Select(b => b.Id).Should().OnlyHaveUniqueItems();
    }
} 