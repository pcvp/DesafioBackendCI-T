using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Application.Base;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Handler for processing DeleteBranchCommand requests
/// </summary>
public class DeleteBranchHandler : BaseCommandHandler, IRequestHandler<DeleteBranchCommand>
{
    private readonly IBranchRepository _branchRepository;

    /// <summary>
    /// Initializes a new instance of DeleteBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public DeleteBranchHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _branchRepository = branchRepository;
    }

    /// <summary>
    /// Handles the DeleteBranchCommand request
    /// </summary>
    /// <param name="command">The DeleteBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public async Task Handle(DeleteBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID {command.Id} not found");

        await _branchRepository.DeleteAsync(command.Id, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit branch deletion transaction");
    }
} 