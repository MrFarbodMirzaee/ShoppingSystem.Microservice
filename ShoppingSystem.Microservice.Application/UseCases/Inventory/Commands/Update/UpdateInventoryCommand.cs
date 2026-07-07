using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.Update;

public sealed record UpdateInventoryCommand(
    Guid InventoryId,
    byte Quantity,
    bool Increase
) : IRequest<Response<bool>>;
