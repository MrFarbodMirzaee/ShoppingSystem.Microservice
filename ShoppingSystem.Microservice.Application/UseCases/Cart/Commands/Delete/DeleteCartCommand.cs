using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Delete;

public sealed record DeleteCartCommand(
    Guid CartId
) : IRequest<Response<bool>>;