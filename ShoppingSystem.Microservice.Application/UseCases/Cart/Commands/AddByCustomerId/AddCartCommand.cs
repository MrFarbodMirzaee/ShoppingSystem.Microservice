using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.AddByCustomerId;

public sealed record AddCartCommand(
    Guid CustomerId,
    Guid UserId
) : IRequest<Response<bool>>;