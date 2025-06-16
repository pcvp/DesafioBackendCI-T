using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem;

/// <summary>
/// Profile for mapping between Application and API AddSaleItem requests
/// </summary>
public class CreateSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for AddSaleItem feature
    /// </summary>
    public CreateSaleItemProfile()
    {
        CreateMap<AddSaleItemRequest, CreateSaleItemCommand>();
        CreateMap<CreateSaleItemResult, AddSaleItemResponse>();
    }
} 