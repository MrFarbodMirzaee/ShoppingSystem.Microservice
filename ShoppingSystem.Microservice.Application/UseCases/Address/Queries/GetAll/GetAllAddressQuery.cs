using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetAll;

public sealed record GetAllAddressQuery(QueryCriteriaRequestDto Criteria)
    : IRequest<Response<PagedResult<AddressResponseDto>>>;