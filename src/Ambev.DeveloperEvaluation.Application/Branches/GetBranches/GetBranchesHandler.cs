using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

/// <summary>
/// Handler for processing GetBranchesCommand requests
/// </summary>
public class GetBranchesHandler : IRequestHandler<GetBranchesCommand, GetBranchesResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetBranchesHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetBranchesHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetBranchesCommand request
    /// </summary>
    /// <param name="command">The GetBranches command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of branches</returns>
    public async Task<GetBranchesResult> Handle(GetBranchesCommand command, CancellationToken cancellationToken)
    {
        var (branches, totalCount) = await _branchRepository.GetPagedAsync(
            command.Page, 
            command.Size, 
            command.Name, 
            cancellationToken);

        var branchDtos = _mapper.Map<List<BranchDto>>(branches);
        var totalPages = (int)Math.Ceiling((double)totalCount / command.Size);

        var result = new GetBranchesResult
        {
            Data = branchDtos,
            CurrentPage = command.Page,
            TotalPages = totalPages,
            TotalCount = totalCount
        };

        return result;
    }
} 