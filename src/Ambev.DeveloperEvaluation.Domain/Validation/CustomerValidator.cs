using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Customer entity that defines validation rules for customer data.
/// </summary>
public class CustomerValidator : AbstractValidator<Customer>
{
    /// <summary>
    /// Initializes a new instance of the CustomerValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Name: Required, length between 2 and 100 characters
    /// - Email: Must be valid format if provided (using EmailValidator)
    /// - Phone: Must match valid phone format if provided
    /// </remarks>
    public CustomerValidator()
    {
        RuleFor(customer => customer.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MinimumLength(2)
            .WithMessage("Customer name must be at least 2 characters long")
            .MaximumLength(100)
            .WithMessage("Customer name cannot be longer than 100 characters");

        RuleFor(customer => customer.Email)
            .SetValidator(new EmailValidator()!)
            .When(customer => !string.IsNullOrEmpty(customer.Email));

        RuleFor(customer => customer.Phone)
            .Matches(@"^\+[1-9]\d{10,14}$")
            .WithMessage("Phone number must start with '+' followed by 11-15 digits")
            .When(customer => !string.IsNullOrEmpty(customer.Phone));
    }
} 