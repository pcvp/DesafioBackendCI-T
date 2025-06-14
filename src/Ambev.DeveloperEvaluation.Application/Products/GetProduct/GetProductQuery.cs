using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Query for retrieving a product by ID
/// </summary>
public class GetProductQuery : IRequest<GetProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to retrieve
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Validates the query using the GetProductQueryValidator
    /// </summary>
    /// <returns>A ValidationResultDetail containing any validation errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new GetProductQueryValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 