using AutoMapper;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAll;

public class GetAllProductQueryHandler
    : IRequestHandler<GetAllProductQuery, Response<PagedResult<ProductResponseDto>>>
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public GetAllProductQueryHandler
    (
        IProductService productService,
        IMapper mapper
    )
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<Response<PagedResult<ProductResponseDto>>> Handle(
        GetAllProductQuery request,
        CancellationToken ct)
    {
        var mappedCriteria = _mapper.Map<QueryCriteria>(request.QueryCriteria);

        var productsResponse = await _productService
            .GetAllAsync(mappedCriteria, ct);

        if (!productsResponse.Succeeded || productsResponse.Data is null)
            return new Response<PagedResult<ProductResponseDto>>(
                "It couldn't retrieve products.",
                productsResponse.Errors);

        var mappedItems = _mapper.Map<List<ProductResponseDto>>(
            productsResponse.Data.Items);

        var pagedResponse = new PagedResult<ProductResponseDto>
        {
            Items = mappedItems,
            TotalCount = productsResponse.Data.TotalCount,
            PageNumber = productsResponse.Data.PageNumber,
            PageSize = productsResponse.Data.PageSize
        };

        return new Response<PagedResult<ProductResponseDto>>(pagedResponse);
    }
}