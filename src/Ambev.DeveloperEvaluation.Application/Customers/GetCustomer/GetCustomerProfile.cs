using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Profile for mapping between Customer entity and GetCustomerResult
/// </summary>
public class GetCustomerProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCustomer operation
    /// </summary>
    public GetCustomerProfile()
    {
        CreateMap<Customer, GetCustomerResult>();
    }
} 