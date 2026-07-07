using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByState;

public sealed record GetByStateAddressQuery(
    string State,
    QueryCriteriaRequestDto QueryCriteriaRequestDto
) : IRequest<Response<PagedResult<AddressResponseDto>>>;