using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Queries.GetById;

public class GetByIdProductQueryHandler
    : IRequestHandler<GetByIdProductQuery, Response<ProductResponseDto>>
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByIdProductQuery> _validator;

    public GetByIdProductQueryHandler
    (
        IProductService productService,
        IMapper mapper,
        IValidator<GetByIdProductQuery> validator
    )
    {
        _productService = productService;
        _mapper = mapper;
        _validator  = validator;
    }

    public async Task<Response<ProductResponseDto>> Handle
    (
        GetByIdProductQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<ProductResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var productResponse = await _productService
            .GetByIdAsync(request.ProductId, ct);

        if (!productResponse.Succeeded || productResponse.Data is null)
            return new Response<ProductResponseDto>
                ("Product not found.");

        var product = _mapper
            .Map<ProductResponseDto>(productResponse.Data);

        return new Response
        <ProductResponseDto>(product);
    }
}