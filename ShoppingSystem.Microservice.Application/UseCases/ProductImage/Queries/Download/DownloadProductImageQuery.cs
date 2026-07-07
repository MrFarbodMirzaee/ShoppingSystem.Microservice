using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.Download;

public sealed record DownloadProductImageQuery(
    Guid Id
) : IRequest<Response<ProductImageDownloadDto>>;