using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Commands.Add;

public sealed record AddProductCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid userId
) : IRequest<Response<bool>>;