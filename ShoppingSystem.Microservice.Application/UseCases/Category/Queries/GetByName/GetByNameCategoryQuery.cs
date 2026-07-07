using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByName;

public sealed record GetByNameCategoryQuery(
    string Name
) : IRequest<Response<CategoryResponseDto>>;