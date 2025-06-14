using AutoMapper;
using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of DeleteProductHandler
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    public DeleteProductHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the DeleteProductCommand request
    /// </summary>
    /// <param name="command">The DeleteProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted successfully</returns>
    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"DeleteProductHandler: Deleting product with ID '{command.Id}'");

        var validator = new DeleteProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // TODO: Implementar lógica real quando domain layer estiver pronto
        // Por enquanto, simula exclusão bem-sucedida
        Console.WriteLine($"DeleteProductHandler: Product deleted successfully");

        return true;
    }
} 