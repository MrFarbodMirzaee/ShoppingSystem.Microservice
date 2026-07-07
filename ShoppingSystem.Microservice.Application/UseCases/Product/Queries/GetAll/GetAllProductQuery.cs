using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAll;

public sealed record GetAllProductQuery(QueryCriteriaRequestDto QueryCriteria)
    : IRequest<Response<PagedResult<ProductResponseDto>>>;