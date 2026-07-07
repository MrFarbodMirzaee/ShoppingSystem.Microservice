using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.HasItem;

public sealed record HasItemCartCommand(
    Guid CartId,
    Guid ProductId
) : IRequest<Response<bool>>;