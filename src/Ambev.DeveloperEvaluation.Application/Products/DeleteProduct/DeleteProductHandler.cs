using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Base;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests
/// </summary>
public class DeleteProductHandler : BaseCommandHandler, IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of DeleteProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public DeleteProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the DeleteProductCommand request
    /// </summary>
    /// <param name="command">The DeleteProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deletion was successful</returns>
    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {command.Id} not found");

        var success = await _productRepository.DeleteAsync(command.Id, cancellationToken);
        if (!success)
            throw new InvalidOperationException($"Failed to delete product with ID {command.Id}");

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit product deletion transaction");

        return true;
    }
} 