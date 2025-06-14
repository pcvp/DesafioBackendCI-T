using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Profile for mapping UpdateBranch operations
/// </summary>
public class UpdateBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateBranch operation
    /// </summary>
    public UpdateBranchProfile()
    {
        CreateMap<UpdateBranchRequest, UpdateBranchCommand>();
        CreateMap<UpdateBranchResult, UpdateBranchResponse>();
    }
} 