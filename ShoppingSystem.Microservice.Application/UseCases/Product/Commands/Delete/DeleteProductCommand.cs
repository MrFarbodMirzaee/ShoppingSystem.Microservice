using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Delete;

public sealed record DeleteProductCommand(
    Guid ProductId
) : IRequest<Response<bool>>;