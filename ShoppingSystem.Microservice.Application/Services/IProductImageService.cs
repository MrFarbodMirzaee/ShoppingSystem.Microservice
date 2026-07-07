using Microsoft.AspNetCore.Http;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

/// <summary>
/// ToDo:add use cases interfaces
/// </summary>
public interface IProductImageService : IRepository<ProductImage>
{
    Task<ProductImage> UploadAsync(
        Guid productId,
        IFormFile file,
        bool isMain,
        CancellationToken ct);
    
    Task<Response<ProductImageDownloadDto>> DownloadAsync(
        Guid id,
        CancellationToken ct);
    
    Task<Response<ProductImageResponseDto>> GetFilePathByIdAsync(
        Guid Id,
        CancellationToken ct);

    Task<bool> ExistsAsync(
        string filePath,
        CancellationToken ct);
}