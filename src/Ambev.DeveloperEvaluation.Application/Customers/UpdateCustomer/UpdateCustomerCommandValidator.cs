using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Validator for UpdateCustomerCommand that defines validation rules for customer update.
/// </summary>
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateCustomerCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Required, must not be empty
    /// - Name: Required, length between 2 and 100 characters
    /// - Email: Must be valid format if provided
    /// - Phone: Must match valid phone format if provided
    /// </remarks>
    public UpdateCustomerCommandValidator()
    {
        RuleFor(customer => customer.Id)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(customer => customer.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .Length(2, 100)
            .WithMessage("Customer name must be between 2 and 100 characters");

        RuleFor(customer => customer.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .When(customer => !string.IsNullOrEmpty(customer.Email));

        RuleFor(customer => customer.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in valid international format")
            .When(customer => !string.IsNullOrEmpty(customer.Phone));
    }
} 