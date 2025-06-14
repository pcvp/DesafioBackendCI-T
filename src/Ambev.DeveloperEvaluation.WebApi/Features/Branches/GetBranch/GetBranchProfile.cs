using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Profile for mapping GetBranch operations
/// </summary>
public class GetBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetBranch operation
    /// </summary>
    public GetBranchProfile()
    {
        CreateMap<GetBranchRequest, GetBranchCommand>();
        CreateMap<GetBranchResult, GetBranchResponse>();
    }
} 