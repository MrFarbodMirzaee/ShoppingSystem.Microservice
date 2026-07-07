using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetOutOfStock;

public sealed record GetOutOfStockInventoryQuery(QueryCriteriaRequestDto QueryCriteriaRequestDto)
    : IRequest<Response<PagedResult<InventoryResponseDto>>>;