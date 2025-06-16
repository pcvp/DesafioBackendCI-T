using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// AutoMapper profile for mapping entities to events
/// </summary>
public class EventMappingProfile : Profile
{
    /// <summary>
    /// Initializes the mapping configuration
    /// </summary>
    public EventMappingProfile()
    {
        CreateMap<Sale, SaleCreatedEvent>()
            .ForMember(dest => dest.EventTimestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Sale, SaleModifiedEvent>()
            .ForMember(dest => dest.EventTimestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Sale, SaleCancelledEvent>()
            .ForMember(dest => dest.EventTimestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Sale, SaleStatusChangedEvent>()
            .ForMember(dest => dest.EventTimestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<SaleItem, SaleItemCancelledEvent>()
            .ForMember(dest => dest.EventTimestamp, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
} 