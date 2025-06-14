using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Profile for mapping UpdateBranch operations between Domain and Application layers
/// </summary>
public class UpdateBranchApplicationProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateBranch operation
    /// </summary>
    public UpdateBranchApplicationProfile()
    {
        CreateMap<Branch, UpdateBranchResult>();
    }
} 