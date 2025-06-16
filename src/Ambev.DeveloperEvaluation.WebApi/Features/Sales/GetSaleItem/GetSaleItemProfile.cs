using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItem;

/// <summary>
/// Profile for mapping GetSaleItem operations
/// </summary>
public class GetSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSaleItem operation
    /// </summary>
    public GetSaleItemProfile()
    {
        CreateMap<GetSaleItemRequest, GetSaleItemQuery>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();
    }
} 