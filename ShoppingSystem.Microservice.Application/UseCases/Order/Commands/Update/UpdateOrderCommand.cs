using MediatR;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.Update;

public sealed record UpdateOrderCommand(
    Guid OrderId,
    OrderStatus Status
) : IRequest<Response<bool>>;