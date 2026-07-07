using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.GetUrl;

/// <summary>
/// ToDo: add compressor image with image sharp
/// </summary>
public class GetUrlProductImageQueryHandler
    : IRequestHandler<GetUrlProductImageQuery, Response<ProductImageResponseDto>>
{
    private readonly IProductImageService _productImageService;
    private readonly IValidator<GetUrlProductImageQuery> _validator;

    public GetUrlProductImageQueryHandler
    (
        IProductImageService productImageService,
        IValidator<GetUrlProductImageQuery> validator
    )
    {
        _productImageService = productImageService;
        _validator = validator;
    }

    public async Task<Response<ProductImageResponseDto>> Handle
    (
        GetUrlProductImageQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
                    .ValidateAsync(request, ct);
        
                if (!validationResult.IsValid)
                {
                    return new Response<ProductImageResponseDto>
                    {
                        Succeeded = false,
                        Message = "Validation failed.",
                        Errors = validationResult.Errors
                            .Select(x => x.ErrorMessage)
                            .ToList(),
                        Data = null
                    };
                }
        
        var imageResponse = await _productImageService
            .GetByIdAsync(
                request.Id,
                ct
            );

        if (!imageResponse.Succeeded || imageResponse.Data is null)
            return new Response<ProductImageResponseDto>(
                "Product image not found."
            );

        var imageUrl = _productImageService
            .GetFilePathByIdAsync(Id: request.Id, ct);

        return new Response<ProductImageResponseDto>(
            imageUrl.Result.Data
        );
    }
}