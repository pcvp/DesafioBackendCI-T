using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

/// <summary>
/// Profile for mapping between Application and API UpdateCustomer requests/responses
/// </summary>
public class UpdateCustomerProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCustomer feature
    /// </summary>
    public UpdateCustomerProfile()
    {
        CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();
        CreateMap<UpdateCustomerResult, UpdateCustomerResponse>();
    }
} 