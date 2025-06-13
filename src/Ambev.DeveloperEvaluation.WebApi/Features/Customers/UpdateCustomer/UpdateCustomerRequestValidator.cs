using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

/// <summary>
/// Validator for UpdateCustomerRequest
/// </summary>
public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateCustomerRequest
    /// </summary>
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(customer => customer.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .Length(2, 100)
            .WithMessage("Customer name must be between 2 and 100 characters");

        RuleFor(customer => customer.Email)
            .SetValidator(new EmailValidator())
            .When(customer => !string.IsNullOrEmpty(customer.Email));

        RuleFor(customer => customer.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in valid international format")
            .When(customer => !string.IsNullOrEmpty(customer.Phone));
    }
} 