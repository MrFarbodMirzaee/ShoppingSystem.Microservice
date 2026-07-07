using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetByProductId;

public sealed record GetByProductIdInventoryQuery(
    Guid ProductId
) : IRequest<Response<InventoryResponseDto>>;