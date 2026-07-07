using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Commands.Clear;

public sealed record ClearCartCommand(
    Guid CartId
) : IRequest<Response<bool>>;