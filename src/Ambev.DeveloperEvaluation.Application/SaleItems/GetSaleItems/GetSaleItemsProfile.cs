using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItems;

/// <summary>
/// Profile for mapping between SaleItem entity and GetSaleItemsResult
/// </summary>
public class GetSaleItemsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSaleItems operation
    /// </summary>
    public GetSaleItemsProfile()
    {
        CreateMap<SaleItem, SaleItemResultDto>();
    }
} 