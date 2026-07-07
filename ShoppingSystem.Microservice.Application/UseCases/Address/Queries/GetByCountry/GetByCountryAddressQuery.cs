using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByCountry;

public sealed record GetByCountryAddressQuery(
    string Country,
    QueryCriteriaRequestDto QueryCriteriaRequestDto
) : IRequest<Response<PagedResult<AddressResponseDto>>>;