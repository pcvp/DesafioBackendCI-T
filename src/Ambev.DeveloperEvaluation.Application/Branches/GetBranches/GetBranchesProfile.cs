using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

/// <summary>
/// Profile for mapping GetBranches operations between Domain and Application layers
/// </summary>
public class GetBranchesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetBranches operation
    /// </summary>
    public GetBranchesProfile()
    {
        CreateMap<Branch, BranchDto>();
    }
} 