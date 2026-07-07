using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Add;

public sealed record AddInventoryCommand(
    Guid ProductId,
    byte Quantity
) : IRequest<Response<bool>>;