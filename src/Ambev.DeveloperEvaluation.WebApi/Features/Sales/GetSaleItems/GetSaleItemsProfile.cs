using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItems;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItems;

/// <summary>
/// Profile for mapping GetSaleItems operations
/// </summary>
public class GetSaleItemsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSaleItems operation
    /// </summary>
    public GetSaleItemsProfile()
    {
        CreateMap<GetSaleItemsRequest, GetSaleItemsQuery>();
        CreateMap<GetSaleItemsResult, GetSaleItemsResponse>();
        CreateMap<SaleItemResultDto, SaleItemDto>();
    }
} 