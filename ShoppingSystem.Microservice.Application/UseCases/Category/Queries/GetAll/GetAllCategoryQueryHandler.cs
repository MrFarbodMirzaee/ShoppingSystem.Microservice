using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetAll;

public sealed class GetAllCategoryQueryHandler
    : IRequestHandler<GetAllCategoryQuery, Response<PagedResult<CategoryResponseDto>>>
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public GetAllCategoryQueryHandler
    (
        ICategoryService categoryService,
        IMapper mapper
    )
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<CategoryResponseDto>>> Handle(
        GetAllCategoryQuery request,
        CancellationToken ct)
    {
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.Criteria);

        var categoriesResponse = await _categoryService
            .GetAllAsync(mappedCriteria, ct);

        if (!categoriesResponse.Succeeded || categoriesResponse.Data is null)
            return new Response<PagedResult<CategoryResponseDto>>(
                "It couldn't retrieve categories.",
                categoriesResponse.Errors);

        var mappedItems = _mapper.Map<List<CategoryResponseDto>>(
            categoriesResponse.Data.Items);

        var pagedResponse = new PagedResult<CategoryResponseDto>
        {
            Items = mappedItems,
            TotalCount = categoriesResponse.Data.TotalCount,
            PageNumber = categoriesResponse.Data.PageNumber,
            PageSize = categoriesResponse.Data.PageSize
        };

        return new Response<PagedResult<CategoryResponseDto>>(pagedResponse);
    }
}