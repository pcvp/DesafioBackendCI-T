using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Handler for processing GetBranchCommand requests
/// </summary>
public class GetBranchHandler : IRequestHandler<GetBranchCommand, GetBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetBranchHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetBranchCommand request
    /// </summary>
    /// <param name="command">The GetBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch details if found</returns>
    public async Task<GetBranchResult> Handle(GetBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID {command.Id} not found");

        var result = _mapper.Map<GetBranchResult>(branch);
        return result;
    }
} 