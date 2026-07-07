using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Delete;

public sealed record DeleteProductImageCommand(
    Guid Id
) : IRequest<Response<bool>>;