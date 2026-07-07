using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetByNumber;

public sealed record GetByNumberOrderQuery(
    string OrderNumber
) : IRequest<Response<OrderResponseDto>>;