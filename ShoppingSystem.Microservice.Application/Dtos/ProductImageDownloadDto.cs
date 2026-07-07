namespace ShoppingSystem.Microservice.Application.Dtos;

public sealed record ProductImageDownloadDto(
    string FileName,
    string ContentType,
    byte[] FileData
);