using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;

/// <summary>
/// Profile for mapping GetBranches operations
/// </summary>
public class GetBranchesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetBranches operation
    /// </summary>
    public GetBranchesProfile()
    {
        CreateMap<GetBranchesRequest, GetBranchesCommand>();
        CreateMap<GetBranchesResult, GetBranchesResponse>();
        CreateMap<BranchDto, BranchSummary>();
    }
} 