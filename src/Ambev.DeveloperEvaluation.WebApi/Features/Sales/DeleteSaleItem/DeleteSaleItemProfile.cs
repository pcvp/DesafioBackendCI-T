using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSaleItem;

/// <summary>
/// Profile for mapping DeleteSaleItem operations
/// </summary>
public class DeleteSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteSaleItem operation
    /// </summary>
    public DeleteSaleItemProfile()
    {
        CreateMap<DeleteSaleItemRequest, DeleteSaleItemCommand>();
    }
} 