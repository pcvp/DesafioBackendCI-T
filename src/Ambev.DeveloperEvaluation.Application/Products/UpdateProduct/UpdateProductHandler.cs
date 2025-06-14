using AutoMapper;
using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for processing UpdateProductCommand requests
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateProductHandler
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateProductHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateProductCommand request
    /// </summary>
    /// <param name="command">The UpdateProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product result</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"UpdateProductHandler: Updating product with ID '{command.Id}', name '{command.Name}', price '{command.Price}'");

        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // TODO: Implementar lógica real quando domain layer estiver pronto
        // Por enquanto, retorna dados mock
        var result = new UpdateProductResult
        {
            Id = command.Id,
            Name = command.Name,
            Price = command.Price,
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            UpdatedAt = DateTime.UtcNow
        };

        Console.WriteLine($"UpdateProductHandler: Product updated successfully");

        return result;
    }
} 