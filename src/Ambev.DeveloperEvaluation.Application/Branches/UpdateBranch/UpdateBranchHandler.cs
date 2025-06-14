using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Handler for processing UpdateBranchCommand requests
/// </summary>
public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateBranchHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateBranchCommand request
    /// </summary>
    /// <param name="command">The UpdateBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated branch details</returns>
    public async Task<UpdateBranchResult> Handle(UpdateBranchCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateBranchCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var branch = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID {command.Id} not found");

        branch.Name = command.Name;
        branch.UpdatedAt = DateTime.UtcNow;

        var validationResultDetail = branch.Validate();
        if (!validationResultDetail.IsValid)
            throw new ValidationException(validationResultDetail.Errors.Select(e => new FluentValidation.Results.ValidationFailure("", e.Detail)));

        var updatedBranch = await _branchRepository.UpdateAsync(branch, cancellationToken);
        var result = _mapper.Map<UpdateBranchResult>(updatedBranch);
        return result;
    }
} 