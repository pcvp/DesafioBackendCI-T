using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;

/// <summary>
/// Profile for mapping between Application and API GetCustomers requests/responses
/// </summary>
public class GetCustomersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCustomers feature
    /// </summary>
    public GetCustomersProfile()
    {
        CreateMap<GetCustomersRequest, GetCustomersCommand>();
        CreateMap<GetCustomersResult, GetCustomersResponse>();
        CreateMap<CustomerSummaryResult, CustomerSummary>();
    }
} 