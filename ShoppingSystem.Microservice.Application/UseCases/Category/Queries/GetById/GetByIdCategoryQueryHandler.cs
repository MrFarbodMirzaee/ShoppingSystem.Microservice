using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetById;

public sealed class GetByIdCategoryQueryHandler
    : IRequestHandler<GetByIdCategoryQuery, Response<CategoryResponseDto>>
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByIdCategoryQuery> _validator;
    
    public GetByIdCategoryQueryHandler
    (
        ICategoryService categoryService,
        IMapper mapper,
        IValidator<GetByIdCategoryQuery> validator
    )
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<CategoryResponseDto>> Handle
    (
        GetByIdCategoryQuery request,
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
            .GetByIdAsync(request.CategoryId, ct);

        if (!categoryResponse.Succeeded || categoryResponse.Data is null)
            return new Response<CategoryResponseDto>
            (
                "Category not found."
            );

        var category = _mapper.Map<CategoryResponseDto>(
            categoryResponse.Data
        );

        return new Response
        <CategoryResponseDto>(category);
    }
}