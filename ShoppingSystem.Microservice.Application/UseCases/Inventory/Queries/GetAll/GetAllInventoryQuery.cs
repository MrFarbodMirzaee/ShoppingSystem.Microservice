using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Queries.GetAll;

public sealed record GetAllInventoryQuery(QueryCriteriaRequestDto QueryCriteria)
    : IRequest<Response<PagedResult<InventoryResponseDto>>>;