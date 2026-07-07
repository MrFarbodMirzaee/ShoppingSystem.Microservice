using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Address.Queries.GetById;

public sealed record GetByIdAddressQuery(
    Guid Id
) : IRequest<Response<AddressResponseDto>>;