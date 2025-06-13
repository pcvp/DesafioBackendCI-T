using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer in the system with contact and profile information.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>
    /// Gets the customer's name.
    /// Must not be null or empty and should be between 2 and 100 characters.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the customer's email address.
    /// Must be a valid email format if provided.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets the customer's phone number.
    /// Must be a valid phone number format if provided.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets whether the customer is active in the system.
    /// Inactive customers cannot be used in new sales.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the date and time when the customer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the customer's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Customer class.
    /// </summary>
    public Customer()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Performs validation of the customer entity using the CustomerValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Name format and length</list>
    /// <list type="bullet">Email format (if provided)</list>
    /// <list type="bullet">Phone number format (if provided)</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new CustomerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Activates the customer account.
    /// Changes the customer's status to Active.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the customer account.
    /// Changes the customer's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the customer's contact information.
    /// </summary>
    /// <param name="name">The new name</param>
    /// <param name="email">The new email</param>
    /// <param name="phone">The new phone</param>
    public void UpdateContactInfo(string name, string? email = null, string? phone = null)
    {
        Name = name;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }
} 