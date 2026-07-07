using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetById;

public sealed record GetByIdOrderQuery(
    Guid OrderId
) : IRequest<Response<OrderResponseDto>>;