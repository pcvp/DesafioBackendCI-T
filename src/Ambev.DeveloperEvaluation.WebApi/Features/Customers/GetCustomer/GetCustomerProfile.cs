using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

/// <summary>
/// Profile for mapping between Application and API GetCustomer requests/responses
/// </summary>
public class GetCustomerProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCustomer feature
    /// </summary>
    public GetCustomerProfile()
    {
        CreateMap<Guid, GetCustomerCommand>()
            .ConstructUsing(id => new GetCustomerCommand(id));
        
        CreateMap<GetCustomerResult, GetCustomerResponse>();
    }
} 