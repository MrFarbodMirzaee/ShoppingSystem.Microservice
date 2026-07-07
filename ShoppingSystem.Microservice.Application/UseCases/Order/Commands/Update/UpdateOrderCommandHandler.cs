using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Events.Order;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.Update;

public class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, Response<bool>>
{
    private readonly IOrderService _orderService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<UpdateOrderCommand> _validator;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(
        IOrderService orderService,
        IUnitOfWorkBase unitOfWork,
        IValidator<UpdateOrderCommand> validator,
        IMediator mediator,
        ILogger<UpdateOrderCommandHandler> logger)
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mediator = mediator;
        _logger = logger;
    }


    public async Task<Response<bool>> Handle(
        UpdateOrderCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting order status update. OrderId: {OrderId}, NewStatus: {NewStatus}.",
            request.OrderId,
            request.Status);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Order update validation failed for OrderId {OrderId}. Errors: {Errors}",
                request.OrderId,
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

        var orderResponse = await _orderService
            .GetByIdAsync(request.OrderId, ct);

        if (!orderResponse.Succeeded || orderResponse.Data is null)
        {
            _logger.LogWarning(
                "Order update failed. OrderId {OrderId} was not found.",
                request.OrderId);

            return new Response<bool>(
                false,
                "Order not found.");
        }

        var order = orderResponse.Data;

        var previousStatus = order.Status;

        order.UpdateStatus(request.Status);

        var result = await _orderService.UpdateAsync(order, ct);

        await _unitOfWork.SaveAsync(ct);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Order update failed. OrderId: {OrderId}. Reason: {Reason}",
                order.Id,
                result.Message);

            return new Response<bool>(result.Message);
        }

        switch (order.Status)
        {
            case OrderStatus.Paid:

                await _mediator.Publish(
                    new OrderConfirmedEvent(
                        order.UserId,
                        order.OrderNumber),
                    ct);

                _logger.LogInformation(
                    "OrderConfirmedEvent published. OrderNumber: {OrderNumber}.",
                    order.OrderNumber);

                break;

            case OrderStatus.Cancelled:

                await _mediator.Publish(
                    new OrderCancelledEvent(
                        order.OrderNumber,
                        "order cancelled",
                        order.UserId),
                    ct);

                _logger.LogInformation(
                    "OrderCancelledEvent published. OrderNumber: {OrderNumber}.",
                    order.OrderNumber);

                break;
        }

        _logger.LogInformation(
            "Order status updated successfully. OrderId: {OrderId}, OrderNumber: {OrderNumber}, PreviousStatus: {PreviousStatus}, NewStatus: {NewStatus}.",
            order.Id,
            order.OrderNumber,
            previousStatus,
            order.Status);

        return new Response<bool>(true);
    }
}