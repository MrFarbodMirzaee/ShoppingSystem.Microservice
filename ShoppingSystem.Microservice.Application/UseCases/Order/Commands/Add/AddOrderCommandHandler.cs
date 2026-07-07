using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Order;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.Add;

public class AddOrderCommandHandler
    : IRequestHandler<AddOrderCommand, Response<bool>>
{
    private readonly IOrderService _orderService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<AddOrderCommand> _validator;
    private readonly IMediator _mediator;
    private readonly IUserQueryService _userQueryService;
    private readonly ILogger<AddOrderCommandHandler> _logger;
    
    public AddOrderCommandHandler
    (
        IOrderService orderService,
        IUnitOfWorkBase unitOfWork,
        IValidator<AddOrderCommand> validator,
        IMediator mediator,
        IUserQueryService userQueryService,
        ILogger<AddOrderCommandHandler> logger
    )
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mediator = mediator;
        _userQueryService = userQueryService;
        _logger = logger;
    }


    public async Task<Response<bool>> Handle(
    AddOrderCommand request,
    CancellationToken ct)
{
    _logger.LogInformation(
        "Starting order creation. OrderNumber: {OrderNumber}, UserId: {UserId}.",
        request.OrderNumber,
        request.UserId);

    var validationResult = await _validator
        .ValidateAsync(request, ct);

    if (!validationResult.IsValid)
    {
        _logger.LogWarning(
            "Order creation validation failed. OrderNumber: {OrderNumber}. Errors: {Errors}",
            request.OrderNumber,
            validationResult.Errors.Select(x => x.ErrorMessage));

        return new Response<bool>
        {
            Succeeded = false,
            Message = "Validation failed.",
            Errors = validationResult.Errors
                .Select(x => x.ErrorMessage)
                .ToList(),
            Data = false
        };
    }

    var userExists = await _userQueryService
        .UserExistsAsync(request.UserId, ct);

    if (!userExists)
    {
        _logger.LogWarning(
            "Order creation failed. User {UserId} does not exist.",
            request.UserId);

        return new Response<bool>(
            false,
            "User does not exist.");
    }

    var exists = await _orderService
        .ExistsByOrderNumberAsync(request.OrderNumber, ct);

    if (exists.Data)
    {
        _logger.LogWarning(
            "Order creation failed. OrderNumber {OrderNumber} already exists.",
            request.OrderNumber);

        return new Response<bool>(
            false,
            "Order already exists.");
    }

    var orderNumber = OrderNumber.Create(
        request.OrderNumber);

    var order = Domain.Aggregates.Order.Order.Create(
        request.UserId,
        orderNumber);

    _orderService.Add(order, ct);

    await _unitOfWork.SaveAsync(ct);

    _logger.LogInformation(
        "Order created successfully. OrderId: {OrderId}, OrderNumber: {OrderNumber}, UserId: {UserId}.",
        order.Id,
        order.OrderNumber,
        order.UserId);

    await _mediator.Publish(
        new OrderCreatedEvent(
            order.OrderNumber,
            order.UserId,
            totalPrice: order.TotalPrice),
        ct);

    _logger.LogInformation(
        "OrderCreatedEvent published. OrderNumber: {OrderNumber}.",
        order.OrderNumber);

    return new Response<bool>(true);
}
}