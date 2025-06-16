using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleItems;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItems;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSaleStatus;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleStatus;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales and sale items operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    #region Sales Operations

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateSaleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<CreateSaleResponse>(response);

        return Created(string.Empty, result);
    }

    /// <summary>
    /// Retrieves a sale by ID
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetSaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSaleRequest { Id = id };
        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSaleQuery>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<GetSaleResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves sales with pagination
    /// </summary>
    /// <param name="page">Page number (starts from 1)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="search">Optional search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sales</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetSalesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSales(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var request = new GetSalesRequest
        {
            Page = page,
            Size = size,
            Search = search
        };

        var validator = new GetSalesRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSalesQuery>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<GetSalesResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <param name="request">The sale update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateSaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<UpdateSaleResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a sale
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteSaleRequest { Id = id };
        var validator = new DeleteSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteSaleCommand>(request);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    #endregion

    #region Sales Status Operations

    /// <summary>
    /// Updates the sale status (Close/Cancel/Pay)
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <param name="request">The status update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleStatus([FromRoute] Guid id, [FromBody] UpdateSaleStatusRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateSaleStatusRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleStatusCommand>(request);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    #endregion

    #region Sale Items Operations

    /// <summary>
    /// Creates a new sale item for an existing sale
    /// </summary>
    /// <param name="saleId">The ID of the sale to add the item to</param>
    /// <param name="request">The sale item data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item details</returns>
    [HttpPost("{saleId}/items")]
    [ProducesResponseType(typeof(CreateSaleItem.AddSaleItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSaleItem([FromRoute] Guid saleId, [FromBody] CreateSaleItem.AddSaleItemRequest request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validator = new CreateSaleItem.AddSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Set the sale ID from the route
        request.SaleId = saleId;

        var command = _mapper.Map<Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem.CreateSaleItemCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, _mapper.Map<CreateSaleItem.AddSaleItemResponse>(response));
    }

    /// <summary>
    /// Retrieves all items from a sale
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="page">Page number (starts from 1)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sale items</returns>
    [HttpGet("{saleId}/items")]
    [ProducesResponseType(typeof(GetSaleItemsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleItems(
        [FromRoute] Guid saleId,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        CancellationToken cancellationToken = default)
    {
        var request = new GetSaleItemsRequest
        {
            SaleId = saleId,
            Page = page,
            Size = size
        };

        var validator = new GetSaleItemsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSaleItemsQuery>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<GetSaleItemsResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific sale item
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The sale item ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item details if found</returns>
    [HttpGet("{saleId}/items/{itemId}")]
    [ProducesResponseType(typeof(GetSaleItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var request = new GetSaleItemRequest 
        { 
            SaleId = saleId,
            Id = itemId 
        };
        var validator = new GetSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSaleItemQuery>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<GetSaleItemResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing sale item
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The sale item ID</param>
    /// <param name="request">The sale item update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale item details</returns>
    [HttpPut("{saleId}/items/{itemId}")]
    [ProducesResponseType(typeof(UpdateSaleItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, [FromBody] UpdateSaleItemRequest request, CancellationToken cancellationToken)
    {
        request.Id = itemId;
        var validator = new UpdateSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleItemCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<UpdateSaleItemResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a sale item
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The sale item ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{saleId}/items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var request = new DeleteSaleItemRequest 
        { 
            SaleId = saleId,
            Id = itemId 
        };
        var validator = new DeleteSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteSaleItemCommand>(request);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    #endregion
} 