using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetByCategory;

public sealed class GetByCategoryProductsQueryHandler
    : IRequestHandler<GetByCategoryProductsQuery,
        Response<PagedResult<ProductResponseDto>>>
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public GetByCategoryProductsQueryHandler(
        IProductService productService,
        IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<ProductResponseDto>>> Handle(
        GetByCategoryProductsQuery request,
        CancellationToken ct)
    {

        var criteria = _mapper.Map<QueryCriteria>(
            request.QueryCriteriaRequestDto
        );

        var result = await _productService.GetByCategoryAsync(
            request.CategoryId,
            criteria,
            ct
        );

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<ProductResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No products found for this category.",
                Errors = result.Errors
            };
        }

        var mappedItems = _mapper.Map<List<ProductResponseDto>>(
            result.Data.Items
        );

        var pagedResponse = new PagedResult<ProductResponseDto>
        {
            Items = mappedItems,
            TotalCount = result.Data.TotalCount,
            PageNumber = result.Data.PageNumber,
            PageSize = result.Data.PageSize
        };

        return new Response<PagedResult<ProductResponseDto>>(pagedResponse);
    }
}