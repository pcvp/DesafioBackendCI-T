using Ambev.DeveloperEvaluation.Application.Base;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Uow;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Handler for processing CreateBranchCommand requests
/// </summary>
public class CreateBranchHandler : BaseCommandHandler, IRequestHandler<CreateBranchCommand, CreateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="unitOfWork">The unit of work</param>
    public CreateBranchHandler(IBranchRepository branchRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateBranchCommand request
    /// </summary>
    /// <param name="command">The CreateBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch result</returns>
    public async Task<CreateBranchResult> Handle(CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateBranchCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var branch = _mapper.Map<Branch>(command);
        var createdBranch = await _branchRepository.CreateAsync(branch, cancellationToken);

        if (!await Commit(cancellationToken))
            throw new InvalidOperationException("Failed to commit branch creation transaction");

        var result = _mapper.Map<CreateBranchResult>(createdBranch);
        return result;
    }
} 