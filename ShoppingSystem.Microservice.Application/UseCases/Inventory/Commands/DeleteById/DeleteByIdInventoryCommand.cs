using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Commands.DeleteById;

public sealed record DeleteByIdInventoryCommand(
    Guid InventoryId
) : IRequest<Response<bool>>;