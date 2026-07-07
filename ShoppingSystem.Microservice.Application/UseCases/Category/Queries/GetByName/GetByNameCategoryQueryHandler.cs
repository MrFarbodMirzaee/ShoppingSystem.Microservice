using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByName;

public sealed class GetByNameCategoryQueryHandler
    : IRequestHandler<GetByNameCategoryQuery,
    Response<CategoryResponseDto>>
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByNameCategoryQuery> _validator;

    public GetByNameCategoryQueryHandler
    (
        ICategoryService categoryService,
        IMapper mapper,
        IValidator<GetByNameCategoryQuery> validator
    )
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<CategoryResponseDto>> Handle
    (
        GetByNameCategoryQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<CategoryResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var categoryResponse = await _categoryService
            .GetByNameAsync(
                request.Name,
                ct);

        if (!categoryResponse.Succeeded || categoryResponse.Data is null)
            return new Response<CategoryResponseDto>(
                "Category not found.");

        var category = _mapper.Map<CategoryResponseDto>(
            categoryResponse.Data);

        return new Response
        <CategoryResponseDto>(
            category);
    }
}