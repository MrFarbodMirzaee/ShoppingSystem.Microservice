using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetByPostalCode;

public sealed record GetByPostalCodeAddressQuery(
    string PostalCode,
    QueryCriteriaRequestDto QueryCriteriaRequestDto
) : IRequest<Response<PagedResult<AddressResponseDto>>>;