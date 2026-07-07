using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Commands.Add;

public sealed record AddOrderCommand(
    Guid UserId,
    string OrderNumber
) : IRequest<Response<bool>>;