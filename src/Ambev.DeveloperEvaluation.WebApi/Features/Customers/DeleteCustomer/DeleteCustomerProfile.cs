using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;

/// <summary>
/// Profile for mapping between Application and API DeleteCustomer requests
/// </summary>
public class DeleteCustomerProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteCustomer feature
    /// </summary>
    public DeleteCustomerProfile()
    {
        CreateMap<Guid, DeleteCustomerCommand>()
            .ConstructUsing(id => new DeleteCustomerCommand(id));
    }
} 