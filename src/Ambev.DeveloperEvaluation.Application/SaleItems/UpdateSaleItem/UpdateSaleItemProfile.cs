using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;

/// <summary>
/// Profile for mapping between SaleItem entity and UpdateSaleItemResult
/// </summary>
public class UpdateSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSaleItem operation
    /// </summary>
    public UpdateSaleItemProfile()
    {
        CreateMap<SaleItem, UpdateSaleItemResult>();
    }
} 