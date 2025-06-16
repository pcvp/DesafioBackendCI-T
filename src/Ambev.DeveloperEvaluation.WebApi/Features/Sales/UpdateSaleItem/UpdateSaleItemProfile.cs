using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleItem;

/// <summary>
/// Profile for mapping UpdateSaleItem operations
/// </summary>
public class UpdateSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSaleItem operation
    /// </summary>
    public UpdateSaleItemProfile()
    {
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();
        CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
    }
} 