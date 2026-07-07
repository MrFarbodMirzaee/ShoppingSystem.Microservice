using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.GetUrl;

public sealed record GetUrlProductImageQuery(
    Guid Id
) : IRequest<Response<ProductImageResponseDto>>;