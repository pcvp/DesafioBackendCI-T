using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleStatus;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleStatus;

/// <summary>
/// Profile for mapping between UpdateSaleStatus API models and Application models
/// </summary>
public class UpdateSaleStatusProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSaleStatus operation
    /// </summary>
    public UpdateSaleStatusProfile()
    {
        CreateMap<UpdateSaleStatusRequest, UpdateSaleStatusCommand>();
    }
} 