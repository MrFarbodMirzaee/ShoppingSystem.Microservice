using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetAll;

public sealed record GetAllCategoryQuery(QueryCriteriaRequestDto Criteria)
    : IRequest<Response<PagedResult<CategoryResponseDto>>>;