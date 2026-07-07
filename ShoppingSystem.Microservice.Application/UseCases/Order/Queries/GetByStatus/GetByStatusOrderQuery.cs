using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetByStatus;

public sealed record GetByStatusOrderQuery(
    OrderStatus Status,
    QueryCriteriaRequestDto QueryCriteriaRequestDto
) : IRequest<Response<PagedResult<OrderResponseDto>>>;