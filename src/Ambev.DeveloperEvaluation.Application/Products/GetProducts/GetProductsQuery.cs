using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Query for retrieving products with pagination
/// </summary>
public class GetProductsQuery : IRequest<GetProductsResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the search term for filtering products by name
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Validates the query using the GetProductsQueryValidator
    /// </summary>
    /// <returns>A ValidationResultDetail containing any validation errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new GetProductsQueryValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 