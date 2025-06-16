using Xunit;
using FluentAssertions;
using NSubstitute;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Domain.Uow;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleItemHandler"/> class.
/// </summary>
public class DeleteSaleItemHandlerTests
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;
    private readonly DeleteSaleItemHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleItemHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteSaleItemHandlerTests()
    {
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new DeleteSaleItemHandler(_saleItemRepository, _saleRepository, _mapper, _unitOfWork, _messagePublisher);
    }
} 