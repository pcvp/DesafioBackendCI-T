using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;

/// <summary>
/// Validator for GetCustomersRequest
/// </summary>
public class GetCustomersRequestValidator : AbstractValidator<GetCustomersRequest>
{
    /// <summary>
    /// Initializes validation rules for GetCustomersRequest
    /// </summary>
    public GetCustomersRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name filter cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email filter must be a valid email address")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
} 