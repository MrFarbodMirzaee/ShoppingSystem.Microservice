using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.DeleteById;

public sealed record DeleteOrderCommand(
    Guid OrderId
) : IRequest<Response<bool>>;