using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetById;

public sealed record GetByIdInventoryQuery(
    Guid InventoryId
) : IRequest<Response<InventoryResponseDto>>;