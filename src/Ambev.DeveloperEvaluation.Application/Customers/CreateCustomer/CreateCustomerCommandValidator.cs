using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Validator for CreateCustomerCommand that defines validation rules for customer creation.
/// </summary>
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateCustomerCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Name: Required, length between 2 and 100 characters
    /// - Email: Must be valid format if provided
    /// - Phone: Must match valid phone format if provided
    /// </remarks>
    public CreateCustomerCommandValidator()
    {
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