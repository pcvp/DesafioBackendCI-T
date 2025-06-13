using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

/// <summary>
/// Profile for mapping between Customer entity and CustomerSummaryResult
/// </summary>
public class GetCustomersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCustomers operation
    /// </summary>
    public GetCustomersProfile()
    {
        CreateMap<Customer, CustomerSummaryResult>();
    }
} 