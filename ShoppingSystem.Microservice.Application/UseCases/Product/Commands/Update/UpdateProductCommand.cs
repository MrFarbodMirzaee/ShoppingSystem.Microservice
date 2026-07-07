using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Update;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    bool IsAvailable
) : IRequest<Response<bool>>;