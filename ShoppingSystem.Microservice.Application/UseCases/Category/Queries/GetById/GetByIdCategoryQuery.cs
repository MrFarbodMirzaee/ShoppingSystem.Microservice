using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetById;

public sealed record GetByIdCategoryQuery(
    Guid CategoryId
) : IRequest<Response<CategoryResponseDto>>;