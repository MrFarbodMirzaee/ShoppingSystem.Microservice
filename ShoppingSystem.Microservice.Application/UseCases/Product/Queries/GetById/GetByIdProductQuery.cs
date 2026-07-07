using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetById;

public sealed record GetByIdProductQuery(
    Guid ProductId
) : IRequest<Response<ProductResponseDto>>;