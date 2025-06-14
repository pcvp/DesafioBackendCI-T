using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command for deleting a product
/// </summary>
public class DeleteProductCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to delete
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Validates the command using the DeleteProductCommandValidator
    /// </summary>
    /// <returns>A ValidationResultDetail containing any validation errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new DeleteProductCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 