using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Profile for mapping GetBranch operations between Domain and Application layers
/// </summary>
public class GetBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetBranch operation
    /// </summary>
    public GetBranchProfile()
    {
        CreateMap<Branch, GetBranchResult>();
    }
} 