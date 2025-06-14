using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

/// <summary>
/// Profile for mapping DeleteBranch operations
/// </summary>
public class DeleteBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteBranch operation
    /// </summary>
    public DeleteBranchProfile()
    {
        CreateMap<DeleteBranchRequest, DeleteBranchCommand>();
    }
} 