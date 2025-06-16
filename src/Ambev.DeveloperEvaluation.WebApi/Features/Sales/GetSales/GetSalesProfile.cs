using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Profile for mapping between Application and API GetSales requests
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSales feature
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesQuery>();
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<SaleResultDto, SaleResponseDto>();
    }
} 