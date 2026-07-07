using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Category.Queries.GetByActive;

public sealed class GetByActiveCategoryQueryHandler
    : IRequestHandler<GetByActiveCategoryQuery,
        Response<PagedResult<CategoryResponseDto>>>
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public GetByActiveCategoryQueryHandler(
        ICategoryService categoryService,
        IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<CategoryResponseDto>>> Handle(
        GetByActiveCategoryQuery request,
        CancellationToken ct)
    {
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteriaRequestDto);

        var result = await _categoryService.GetActiveCategoriesAsync(
            mappedCriteria,
            ct);

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<CategoryResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No active categories found.",
                Errors = result.Errors
            };
        }

        var mappedItems = _mapper.Map<List<CategoryResponseDto>>(result.Data.Items);

        var pagedResponse = new PagedResult<CategoryResponseDto>
        {
            Items = mappedItems,
            TotalCount = result.Data.TotalCount,
            PageNumber = result.Data.PageNumber,
            PageSize = result.Data.PageSize
        };

        return new Response<PagedResult<CategoryResponseDto>>(pagedResponse);
    }
}