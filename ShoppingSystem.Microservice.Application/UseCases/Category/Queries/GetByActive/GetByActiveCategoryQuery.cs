using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByActive;

public sealed record GetByActiveCategoryQuery(QueryCriteriaRequestDto QueryCriteriaRequestDto)
    : IRequest<Response<PagedResult<CategoryResponseDto>>>;