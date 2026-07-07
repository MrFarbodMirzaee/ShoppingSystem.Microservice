using AutoMapper;
using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// ToDo: check this 
/// </summary>
[ScopedService]
public class ProductImageRepository
    :  Repository<ProductImage> ,IProductImageService
{
    private readonly IMapper _mapper;
    
    public ProductImageRepository
        (AppDbContext context,IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<ProductImage> UploadAsync(
        Guid productId,
        IFormFile file,
        bool isMain,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            throw new InvalidOperationException("File is empty.");

        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, ct);

        var image = new ProductImage(
            productId,
            file.FileName,
            "products/" + file.FileName, 
            file.ContentType,
            memoryStream.ToArray(),
            file.Length,
            isMain
        );

        await DbSet.AddAsync(image, ct);

        return image;
    }

    public async Task<Response<ProductImageDownloadDto>> DownloadAsync(Guid id, CancellationToken ct)
    {
        var image = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (image is null)
            return new Response
                <ProductImageDownloadDto>("Image not found.");

        var dto = _mapper.Map<ProductImageDownloadDto>(image);

        return new Response
            <ProductImageDownloadDto>(dto);
    }

    public async Task<Response<ProductImageResponseDto>> GetFilePathByIdAsync(Guid Id, CancellationToken ct)
    {
        var image = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == Id, ct);

        if (image is null)
            return new Response<ProductImageResponseDto>(
                "Product image not found.");

        var dto = _mapper.Map<ProductImageResponseDto>(image);

        return new Response
        <ProductImageResponseDto>(dto);
    }

    public async Task<bool> ExistsAsync(
        string filePath,
        CancellationToken ct)
    {
        return await DbSet
            .AsNoTracking()
            .AnyAsync(
                x => x.FilePath == filePath,
                ct);
    }
}