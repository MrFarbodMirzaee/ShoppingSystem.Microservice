using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;

namespace ShoppingSystem.Microservice.Application.Profiles.ProductImage;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        #region Queries
        CreateMap<Domain.Entities.ProductImage, ProductImageDownloadDto>()
            .ForCtorParam(nameof(ProductImageDownloadDto.FileName),
                opt => opt.MapFrom(src => src.FileName))
            .ForCtorParam(nameof(ProductImageDownloadDto.ContentType),
                opt => opt.MapFrom(src => src.ContentType))
            .ForCtorParam(nameof(ProductImageDownloadDto.FileData),
                opt => opt.MapFrom(src => src.Data));


        CreateMap<Domain.Entities.ProductImage, ProductImageResponseDto>()
            .ForCtorParam(nameof(ProductImageResponseDto.Url),
                opt => opt.MapFrom(src => src.FilePath));

        #endregion
    }
}