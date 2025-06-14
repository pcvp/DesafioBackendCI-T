using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Command for updating an existing product
/// </summary>
public class UpdateProductCommand : IRequest<UpdateProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Validates the command using the UpdateProductCommandValidator
    /// </summary>
    /// <returns>A ValidationResultDetail containing any validation errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 