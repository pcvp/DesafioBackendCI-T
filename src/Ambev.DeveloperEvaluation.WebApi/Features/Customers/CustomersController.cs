using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomers;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers;

/// <summary>
/// Controller for managing customer operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CustomersController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CustomersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="request">The customer creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCustomerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCustomerCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateCustomerResponse>
        {
            Success = true,
            Message = "Customer created successfully",
            Data = _mapper.Map<CreateCustomerResponse>(response)
        });
    }

    /// <summary>
    /// Retrieves all customers with optional filtering
    /// </summary>
    /// <param name="page">Page number for pagination</param>
    /// <param name="size">Page size for pagination</param>
    /// <param name="name">Optional name filter</param>
    /// <param name="email">Optional email filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCustomersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCustomers(
        [FromQuery] int page = 1, 
        [FromQuery] int size = 10,
        [FromQuery] string? name = null,
        [FromQuery] string? email = null,
        CancellationToken cancellationToken = default)
    {
        var request = new GetCustomersRequest 
        { 
            Page = page, 
            Size = size, 
            Name = name, 
            Email = email 
        };

        var validator = new GetCustomersRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetCustomersCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<GetCustomersResponse>
        {
            Success = true,
            Message = "Customers retrieved successfully",
            Data = _mapper.Map<GetCustomersResponse>(response)
        });
    }

    /// <summary>
    /// Retrieves a customer by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCustomerRequest { Id = id };
        var validator = new GetCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetCustomerCommand>(request.Id);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<GetCustomerResponse>
        {
            Success = true,
            Message = "Customer retrieved successfully",
            Data = _mapper.Map<GetCustomerResponse>(response)
        });
    }

    /// <summary>
    /// Updates an existing customer
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    /// <param name="request">The customer update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateCustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCustomerCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UpdateCustomerResponse>
        {
            Success = true,
            Message = "Customer updated successfully",
            Data = _mapper.Map<UpdateCustomerResponse>(response)
        });
    }

    /// <summary>
    /// Deletes a customer by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the customer was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCustomerRequest { Id = id };
        var validator = new DeleteCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteCustomerCommand>(request.Id);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Customer deleted successfully"
        });
    }
} 