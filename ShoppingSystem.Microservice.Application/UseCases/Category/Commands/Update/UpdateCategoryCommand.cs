using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Update;

public sealed record UpdateCategoryCommand(
    Guid CategoryId,
    string Name,
    string Description
) : IRequest<Response<bool>>;