using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Add;

public sealed record AddCategoryCommand(
    string Name,
    string Description,
    Guid? ParentCategoryId,
    Guid UserId 
) : IRequest<Response<bool>>;