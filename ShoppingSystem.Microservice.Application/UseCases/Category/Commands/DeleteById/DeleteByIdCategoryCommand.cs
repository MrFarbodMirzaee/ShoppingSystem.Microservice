using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.DeleteById;

public sealed record DeleteByIdCategoryCommand(
    Guid CategoryId
) : IRequest<Response<bool>>;