using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleStatus;

/// <summary>
/// Command for updating sale status
/// </summary>
public class UpdateSaleStatusCommand : IRequest
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the new status for the sale
    /// Valid values: "Close", "Cancel", "Pay"
    /// </summary>
    public SaleStatusEnum Status { get; set; }

    /// <summary>
    /// Validates the command using the UpdateSaleStatusCommandValidator
    /// </summary>
    /// <returns>Validation result with any errors found</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new UpdateSaleStatusCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 