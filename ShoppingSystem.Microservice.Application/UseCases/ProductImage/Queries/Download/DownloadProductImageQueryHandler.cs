using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Queries.Download;

public class DownloadProductImageQueryHandler
    : IRequestHandler<DownloadProductImageQuery, Response<ProductImageDownloadDto>>
{
    private readonly IProductImageService _productImageService;
    private readonly IValidator<DownloadProductImageQuery> _validator;

    public DownloadProductImageQueryHandler
    (
        IProductImageService productImageService,
        IValidator<DownloadProductImageQuery> validator
    )
    {
        _productImageService = productImageService;
        _validator = validator;
    }


    public async Task<Response<ProductImageDownloadDto>> Handle
    (
        DownloadProductImageQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<ProductImageDownloadDto>
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
            return new Response<ProductImageDownloadDto>(
                "Product image not found."
            );


        var image = imageResponse.Data;


        var fileData = await _productImageService
            .DownloadAsync(
                image.Id,
                ct
            );


        return new Response
        <ProductImageDownloadDto>(
            fileData.Data
        );
    }
}