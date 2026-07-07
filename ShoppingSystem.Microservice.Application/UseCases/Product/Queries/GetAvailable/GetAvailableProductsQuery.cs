using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAvailable;

public sealed record GetAvailableProductsQuery(QueryCriteriaRequestDto QueryCriteriaRequestDto)
    : IRequest<Response<PagedResult<ProductResponseDto>>>;