using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetLowStock;

public sealed record GetLowStockInventoryQuery
    (int lowStockInventories,QueryCriteriaRequestDto QueryCriteriaRequestDto)
    : IRequest<Response<PagedResult<InventoryResponseDto>>>;