using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Queries.GetByCustomerId;

public sealed record GetByCustomerIdCartQuery(
    Guid CustomerId
) : IRequest<Response<CartResponseDto>>;