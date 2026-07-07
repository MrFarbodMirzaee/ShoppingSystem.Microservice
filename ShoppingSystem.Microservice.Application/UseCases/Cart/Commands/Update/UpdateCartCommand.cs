using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Update;

public sealed record UpdateCartCommand(
    Guid CustomerId,
    Guid ProductId,
    byte Quantity
) : IRequest<Response<bool>>;