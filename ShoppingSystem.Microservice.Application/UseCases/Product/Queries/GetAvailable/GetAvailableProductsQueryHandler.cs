using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetAvailable;

public sealed class GetAvailableProductsQueryHandler
    : IRequestHandler<GetAvailableProductsQuery,
        Response<PagedResult<ProductResponseDto>>>
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetAvailableProductsQuery> _validator;

    public GetAvailableProductsQueryHandler(
        IProductService productService,
        IMapper mapper,
        IValidator<GetAvailableProductsQuery> validator)
    {
        _productService = productService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PagedResult<ProductResponseDto>>> Handle(
        GetAvailableProductsQuery request,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<PagedResult<ProductResponseDto>>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            };
        }

        var criteria = _mapper.Map<QueryCriteria>(
            request.QueryCriteriaRequestDto
        );

        var result = await _productService.GetAvailableProductsAsync(
            criteria,
            ct
        );

        if (!result.Succeeded || result.Data is null)
        {
            return new Response<PagedResult<ProductResponseDto>>
            {
                Succeeded = false,
                Message = result.Message ?? "No available products found.",
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