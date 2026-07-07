using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.DeleteById;

public class DeleteOrderCommandHandler
    : IRequestHandler<DeleteOrderCommand, Response<bool>>
{
    private readonly IOrderService _orderService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IValidator<DeleteOrderCommand> _validator;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler
    (
        IOrderService orderService,
        IUnitOfWorkBase unitOfWork,
        IValidator<DeleteOrderCommand> validator,
        ILogger<DeleteOrderCommandHandler> logger
    )
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        DeleteOrderCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting order deletion. OrderId: {OrderId}.",
            request.OrderId);

        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Order deletion validation failed for OrderId {OrderId}. Errors: {Errors}",
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
                "Order deletion failed. OrderId {OrderId} was not found.",
                request.OrderId);

            return new Response<bool>(
                false,
                "Order not found.");
        }

        var order = orderResponse.Data;

        await _orderService.DeleteByIdAsync(
            order.Id,
            ct);

        await _unitOfWork.SaveAsync(ct);

        _logger.LogInformation(
            "Order deleted successfully. OrderId: {OrderId}, OrderNumber: {OrderNumber}, UserId: {UserId}.",
            order.Id,
            order.OrderNumber,
            order.UserId);

        return new Response<bool>(true);
    }
}